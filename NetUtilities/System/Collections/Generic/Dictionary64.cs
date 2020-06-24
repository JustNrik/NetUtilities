using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NetUtilities;

namespace System.Collections.Generic
{
    public sealed class Dictionary64<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : notnull
    {
        private struct Entry
        {
            public ulong hashCode;
            public int next;
            public TKey key;          
            public TValue value;       
        }

        private unsafe static class UnsafeX
        {
            public static bool IsNullRef<T>(ref T source)
                => Unsafe.AsPointer(ref source) is null;
            internal static ref T NullRef<T>()
                => ref Unsafe.AsRef<T>(null);
        }

        private static partial class HashHelpers
        {
            public const uint HashCollisionThreshold = 100;
            public const int MaxPrimeArrayLength = 0x7FEFFFFD;
            public const int HashPrime = 101;

            private static readonly int[] s_primes =
            {
                3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
                1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
                17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
                187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
                1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
            };

            public static bool IsPrime(int candidate)
            {
                if ((candidate & 1) == 0)
                    return candidate == 2;

                var limit = (nint)Math.Sqrt(candidate);

                for (nint divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }

                return true;
            }

            public static int GetPrime(int min)
            {
                Ensure.NotOutOfRange(min >= 0, min);

                foreach (var prime in s_primes)
                {
                    if (prime >= min)
                        return prime;
                }

                for (var i = (min | 1); i < int.MaxValue; i += 2)
                {
                    if (IsPrime(i) && ((i - 1) % HashPrime is not 0))
                        return i;
                }
                return min;
            }

            public static int ExpandPrime(int oldSize)
            {
                var newSize = 2 * oldSize;

                if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
                    return MaxPrimeArrayLength;

                return GetPrime(newSize);
            }

            public static ulong GetFastModMultiplier(uint divisor)
                => ulong.MaxValue / divisor + 1;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint FastMod(ulong hashcode, uint divisor, ulong multiplier)
                => (uint)(((((multiplier * hashcode) >> 32) + 1) * divisor) >> 32);
        }

        internal enum InsertionBehavior : byte
        {
            None = 0,
            OverwriteExisting = 1,
            ThrowOnExisting = 2
        }

        private int[]? _buckets;
        private Entry[]? _entries;
        private ulong _fastModMultiplier;
        private int _count;
        private int _freeList;
        private int _freeCount;
        private int _version;
        private IEqualityComparer64<TKey>? _comparer;
        private KeyCollection? _keys;
        private ValueCollection? _values;
        private const int StartOfFreeList = -3;

        public IEqualityComparer64<TKey> Comparer
            => _comparer is null
            ? EqualityComparer64<TKey>.Default
            : _comparer;

        public int Count
            => _count - _freeCount;

        public KeyCollection Keys
            => _keys ??= new KeyCollection(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
            => _keys ??= new KeyCollection(this);

        public ValueCollection Values
            => _values ??= new ValueCollection(this);

        ICollection<TValue> IDictionary<TKey, TValue>.Values
            => _values ??= new ValueCollection(this);

        public TValue this[TKey key]
        {
            get
            {
                ref var value = ref FindValue(key);
                if (!UnsafeX.IsNullRef(ref value))
                    return value;

                throw new KeyNotFoundException();
            }
            set => TryInsert(key, value, InsertionBehavior.OverwriteExisting);
        }

        public Dictionary64() : this(0, null) { }

        public Dictionary64(int capacity) : this(capacity, null) { }

        public Dictionary64(IEqualityComparer64<TKey>? comparer) : this(0, comparer) { }

        public Dictionary64(int capacity, IEqualityComparer64<TKey>? comparer)
        {
            Ensure.NotOutOfRange(capacity >= 0, capacity);

            if (capacity > 0) 
                Initialize(capacity);

            if (comparer is not null && comparer != EqualityComparer64<TKey>.Default)
                _comparer = comparer;
        }

        public Dictionary64(IDictionary<TKey, TValue> dictionary) : this(dictionary, null) { }

        public Dictionary64(IDictionary<TKey, TValue> dictionary, IEqualityComparer64<TKey>? comparer) :
            this(dictionary is not null ? dictionary.Count : 0, comparer)
        {
            Ensure.NotNull(dictionary);

            if (dictionary.GetType() == typeof(Dictionary64<TKey, TValue>))
            {
                var d = (Dictionary64<TKey, TValue>)dictionary;
                var count = d._count;
                var entries = d._entries;

                foreach (var entry in entries)
                {
                    if (entry.next >= -1)
                        Add(entry.key, entry.value);
                }

                return;
            }

            foreach (var (key, value) in dictionary)
                Add(key, value);
        }

        public Dictionary64(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null) { }

        public Dictionary64(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer64<TKey>? comparer) :
            this((collection as ICollection<KeyValuePair<TKey, TValue>>)?.Count ?? 0, comparer)
        {
            Ensure.NotNull(collection);

            foreach (var pair in collection)
                Add(pair.Key, pair.Value);
        }

        public void Add(TKey key, TValue value)
            => TryInsert(key, value, InsertionBehavior.ThrowOnExisting);

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
            => Add(keyValuePair.Key, keyValuePair.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            ref TValue value = ref FindValue(keyValuePair.Key);
            return !UnsafeX.IsNullRef(ref value) && EqualityComparer<TValue>.Default.Equals(value, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            ref TValue value = ref FindValue(keyValuePair.Key);
            if (!UnsafeX.IsNullRef(ref value) && EqualityComparer<TValue>.Default.Equals(value, keyValuePair.Value))
            {
                Remove(keyValuePair.Key);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            var count = _count;

            if (count > 0)
            {
                Array.Clear(_buckets, 0, _buckets.Length);

                _count = 0;
                _freeList = -1;
                _freeCount = 0;

                Array.Clear(_entries, 0, count);
            }
        }

        public bool ContainsKey(TKey key)
            => !UnsafeX.IsNullRef(ref FindValue(key));

        public bool ContainsValue(TValue value)
        {
            var entries = _entries;
            var comparer = EqualityComparer<TValue>.Default;

            if (value is null)
            {
                foreach (var entry in entries)
                {
                    if (entry.next >= -1 && entry.value is null) 
                        return true;
                }
            }
            else
            {
                if (typeof(TValue).IsValueType)
                {
                    foreach (var entry in entries)
                    {
                        if (entry.next >= -1 && comparer.Equals(entry.value, value)) 
                            return true;
                    }
                }
                else
                {
                    foreach (var entry in entries)
                    {
                        if (entry.next >= -1 && comparer.Equals(entry.value, value)) 
                            return true;
                    }
                }
            }
            return false;
        }

        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            Ensure.NotNull(array);
            Ensure.NotOutOfRange((uint)index <= (uint)array.Length, index);
            Ensure.NotOutOfRange(array.Length - index >= Count, index);

            var count = _count;
            var entries = _entries;

            foreach (var entry in entries)
            {
                if (entry.next >= -1)
                    array[index++] = new KeyValuePair<TKey, TValue>(entry.key, entry.value);
            }
        }

        public Enumerator GetEnumerator()
            => new Enumerator(this, Enumerator.KeyValuePair);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => new Enumerator(this, Enumerator.KeyValuePair);

        private ref TValue FindValue(TKey key)
        {
            Ensure.NotNull(key);

            ref var entry = ref UnsafeX.NullRef<Entry>();

            if (_buckets is not null)
            {
                var comparer = _comparer;

                if (comparer is null)
                {
                    comparer = EqualityComparer64<TKey>.Default;

                    var hashCode = (ulong)comparer.GetHashCode64(key);
                    var i = GetBucket(hashCode);
                    var entries = _entries;
                    var collisionCount = 0u;

                    if (typeof(TKey).IsValueType)
                    {
                        i--;

                        do
                        {
                            if ((uint)i >= (uint)entries.Length)
                                goto ReturnNotFound;

                            entry = ref entries[i];

                            if (entry.hashCode == hashCode && comparer.Equals(entry.key, key))
                                goto ReturnFound;

                            i = entry.next;

                            collisionCount++;
                        } while (collisionCount <= (uint)entries.Length);

                        goto ConcurrentOperation;
                    }
                    else
                    {
                        i--;

                        do
                        {
                            if ((uint)i >= (uint)entries.Length)
                                goto ReturnNotFound;

                            entry = ref entries[i];

                            if (entry.hashCode == hashCode && comparer.Equals(entry.key, key))
                                goto ReturnFound;

                            i = entry.next;

                            collisionCount++;
                        } while (collisionCount <= (uint)entries.Length);

                        goto ConcurrentOperation;
                    }
                }
                else
                {
                    var hashCode = (ulong)comparer.GetHashCode64(key);
                    var i = GetBucket(hashCode);
                    var entries = _entries;
                    var collisionCount = 0u;
                    
                    i--;
                    do
                    {
                        if ((uint)i >= (uint)entries.Length)
                            goto ReturnNotFound;

                        entry = ref entries[i];

                        if (entry.hashCode == hashCode && comparer.Equals(entry.key, key))
                            goto ReturnFound;

                        i = entry.next;

                        collisionCount++;
                    } while (collisionCount <= (uint)entries.Length);

                    goto ConcurrentOperation;
                }
            }

            goto ReturnNotFound;

            ConcurrentOperation:
            Ensure.CanOperate(false, "Concurrent operations are not supported.");
            ReturnFound:
            ref var value = ref entry.value;
            Return:
            return ref value;
            ReturnNotFound:
            value = ref UnsafeX.NullRef<TValue>();
            goto Return;
        }

        private int Initialize(int capacity)
        {
            var size = HashHelpers.GetPrime(capacity);
            var buckets = new int[size];
            var entries = new Entry[size];
            
            _freeList = -1;
            _fastModMultiplier = HashHelpers.GetFastModMultiplier((uint)size);
            _buckets = buckets;
            _entries = entries;

            return size;
        }

        private bool TryInsert(TKey key, TValue value, InsertionBehavior behavior)
        {
            Ensure.NotNull(key);

            if (_buckets is null)
                Initialize(0);

            var entries = _entries;
            var comparer = _comparer;
            var hashCode = (ulong)(comparer?.GetHashCode64(key) ?? EqualityComparer64<TKey>.Default.GetHashCode64(key));

            var collisionCount = 0u;
            ref var bucket = ref GetBucket(hashCode);
            var i = bucket - 1;

            if (comparer is null)
            {
                comparer = EqualityComparer64<TKey>.Default;
                if (typeof(TKey).IsValueType)
                {
                    while (true)
                    {
                        if ((uint)i >= (uint)entries.Length)
                            break;

                        var currentEntry = entries[i];

                        if (currentEntry.hashCode == hashCode && comparer.Equals(currentEntry.key, key))
                        {
                            if (behavior == InsertionBehavior.OverwriteExisting)
                            {
                                currentEntry.value = value;
                                return true;
                            }

                            if (behavior == InsertionBehavior.ThrowOnExisting)
                                Ensure.CanOperate(false, "Cannot add a duplicate key.");

                            return false;
                        }

                        i = entries[i].next;
                        collisionCount++;

                        if (collisionCount > (uint)entries.Length)
                            Ensure.CanOperate(false, "Concurrent operations are not supported.");
                    }
                }
                else
                {
                    while (true)
                    {
                        if ((uint)i >= (uint)entries.Length)
                            break;

                        var currentEntry = entries[i];

                        if (currentEntry.hashCode == hashCode && comparer.Equals(currentEntry.key, key))
                        {
                            if (behavior == InsertionBehavior.OverwriteExisting)
                            {
                                currentEntry.value = value;
                                return true;
                            }

                            if (behavior == InsertionBehavior.ThrowOnExisting)
                                Ensure.CanOperate(false, "Cannot add a duplicate key.");

                            return false;
                        }

                        i = currentEntry.next;
                        collisionCount++;

                        if (collisionCount > (uint)entries.Length)
                            Ensure.CanOperate(false, "Concurrent operations are not supported.");
                    }
                }
            }
            else
            {
                while (true)
                {
                    if ((uint)i >= (uint)entries.Length)
                        break;

                    var currentEntry = entries[i];

                    if (currentEntry.hashCode == hashCode && comparer.Equals(currentEntry.key, key))
                    {
                        if (behavior == InsertionBehavior.OverwriteExisting)
                        {
                            currentEntry.value = value;
                            return true;
                        }

                        if (behavior == InsertionBehavior.ThrowOnExisting)
                            Ensure.CanOperate(false, "Cannot add a duplicate key.");

                        return false;
                    }

                    i = currentEntry.next;
                    collisionCount++;

                    if (collisionCount > (uint)entries.Length)
                        Ensure.CanOperate(false, "Concurrent operations are not supported.");
                }
            }

            var updateFreeList = false;
            int index;

            if (_freeCount > 0)
            {
                index = _freeList;
                updateFreeList = true;
                _freeCount--;
            }
            else
            {
                var count = _count;

                if (count == entries.Length)
                {
                    Resize();
                    bucket = ref GetBucket(hashCode);
                }

                index = count;
                _count = count + 1;
                entries = _entries;
            }

            ref var entry = ref entries![index];

            if (updateFreeList)
                _freeList = StartOfFreeList - entries[_freeList].next;

            entry.hashCode = hashCode;
            entry.next = bucket - 1;
            entry.key = key;
            entry.value = value;
            bucket = index + 1;
            _version++;

            if (!typeof(TKey).IsValueType && collisionCount > HashHelpers.HashCollisionThreshold)
            {
                _comparer = null;
                Resize(entries.Length, true);
            }

            return true;
        }

        private void Resize()
            => Resize((int)HashHelpers.ExpandPrime(_count), false);

        private void Resize(int newSize, bool forceNewHashCodes)
        {
            var entries = new Entry[newSize];
            var count = _count;

            Array.Copy(_entries, entries, count);

            if (!typeof(TKey).IsValueType && forceNewHashCodes)
            {
                for (nint i = 0; i < entries.Length; i++)
                {
                    if (entries[i].next >= -1)
                        entries[i].hashCode = (ulong)EqualityComparer64<TKey>.Default.GetHashCode64(entries[i].key);
                }
            }
            
            _buckets = new int[newSize];
            _fastModMultiplier = HashHelpers.GetFastModMultiplier((uint)newSize);

            for (int i = 0; i < count; i++)
            {
                var entry = entries[i];

                if (entry.next >= -1)
                {
                    ref var bucket = ref GetBucket(entry.hashCode);
                    entries[i].next = bucket - 1;
                    bucket = i + 1;
                }
            }

            _entries = entries;
        }

        public bool Remove(TKey key)
        {
            Ensure.NotNull(key);

            if (_buckets is not null)
            {
                var collisionCount = 0u;
                var hashCode = (ulong)(_comparer?.GetHashCode64(key) ?? EqualityComparer64<TKey>.Default.GetHashCode64(key));
                ref var bucket = ref GetBucket(hashCode);
                var entries = _entries;
                var last = -1;
                var i = bucket - 1;

                while (i >= 0)
                {
                    ref var entry = ref entries[i];

                    if (entry.hashCode == hashCode && (_comparer?.Equals(entry.key, key) ?? EqualityComparer64<TKey>.Default.Equals(entry.key, key)))
                    {
                        if (last < 0)
                            bucket = entry.next + 1;
                        else
                            entries[last].next = entry.next;

                        entry.next = StartOfFreeList - _freeList;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                            entry.key = default!;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                            entry.value = default!;

                        _freeList = i;
                        _freeCount++;
                        return true;
                    }

                    last = i;
                    i = entry.next;
                    collisionCount++;

                    if (collisionCount > (uint)entries.Length)
                        Ensure.CanOperate(false, "Concurrent operations are not supported.");
                }
            }

            return false;
        }

        public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            Ensure.NotNull(key);

            if (_buckets is not null)
            {
                var collisionCount = 0u;
                var hashCode = (ulong)(_comparer?.GetHashCode64(key) ?? EqualityComparer64<TKey>.Default.GetHashCode64(key));
                ref var bucket = ref GetBucket(hashCode);
                var entries = _entries;
                var last = -1;
                var i = bucket - 1;

                while (i >= 0)
                {
                    ref var entry = ref entries[i];

                    if (entry.hashCode == hashCode && (_comparer?.Equals(entry.key, key) ?? EqualityComparer64<TKey>.Default.Equals(entry.key, key)))
                    {
                        if (last < 0)
                            bucket = entry.next + 1;
                        else
                            entries[last].next = entry.next;

                        value = entry.value;
                        entry.next = StartOfFreeList - _freeList;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                            entry.key = default!;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                            entry.value = default!;

                        _freeList = i;
                        _freeCount++;
                        return true;
                    }

                    last = i;
                    i = entry.next;

                    collisionCount++;
                    if (collisionCount > (uint)entries.Length)
                        Ensure.CanOperate(false, "Concurrent operations are not supported.");
                }
            }

            value = default!;
            return false;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            ref var valRef = ref FindValue(key);

            if (!UnsafeX.IsNullRef(ref valRef))
            {
                value = valRef;
                return true;
            }

            value = default!;
            return false;
        }

        public bool TryAdd(TKey key, TValue value)
            => TryInsert(key, value, InsertionBehavior.None);

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
            => CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this, Enumerator.KeyValuePair);

        public int EnsureCapacity(int capacity)
        {
            Ensure.NotOutOfRange(capacity >= 0, capacity);

            var currentCapacity = _entries is null ? 0 : _entries.Length;

            if (currentCapacity >= capacity)
                return currentCapacity;

            _version++;

            if (_buckets is null)
                return Initialize(capacity);

            var newSize = (int)HashHelpers.GetPrime(capacity);

            Resize(newSize, forceNewHashCodes: false);
            return newSize;
        }

        public void TrimExcess()
            => TrimExcess(Count);

        public void TrimExcess(int capacity)
        {
            Ensure.NotOutOfRange(capacity >= Count, capacity);

            var newSize = (int)HashHelpers.GetPrime(capacity);
            var oldEntries = _entries;
            var currentCapacity = oldEntries is null ? 0 : oldEntries.Length;

            if (newSize >= currentCapacity)
                return;

            _version++;
            Initialize(newSize);

            var oldCount = _count;
            var entries = _entries;
            var count = 0;

            for (nint i = 0; i < oldCount; i++)
            {
                var oldEntry = oldEntries[i];
                var hashCode = oldEntry.hashCode;

                if (oldEntry.next >= -1)
                {
                    ref var entry = ref entries![count];
                    ref var bucket = ref GetBucket(hashCode);

                    entry = oldEntry;
                    entry.next = bucket - 1;
                    bucket = count + 1;
                    count++;
                }
            }

            _count = count;
            _freeCount = 0;
        }

        private static bool IsCompatibleKey(object key)
            => Ensure.NotNull(key) is TKey;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref int GetBucket(ulong hashCode)
        {
            var buckets = _buckets!;
            return ref buckets[HashHelpers.FastMod(hashCode, (uint)buckets.Length, _fastModMultiplier)];
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly Dictionary64<TKey, TValue> _dictionary;
            private readonly int _version;
            private int _index;
            private KeyValuePair<TKey, TValue> _current;
            private readonly int _getEnumeratorRetType; 

            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;

            internal Enumerator(Dictionary64<TKey, TValue> dictionary, int getEnumeratorRetType)
            {
                _dictionary = dictionary;
                _version = dictionary._version;
                _index = 0;
                _getEnumeratorRetType = getEnumeratorRetType;
                _current = default;
            }

            public bool MoveNext()
            {
                Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                while ((uint)_index < (uint)_dictionary._count)
                {
                    ref var entry = ref _dictionary._entries![_index++];

                    if (entry.next >= -1)
                    {
                        _current = new KeyValuePair<TKey, TValue>(entry.key, entry.value);
                        return true;
                    }
                }

                _index = _dictionary._count + 1;
                _current = default;
                return false;
            }

            public KeyValuePair<TKey, TValue> Current => _current;

            public void Dispose()
            {
            }

            object? IEnumerator.Current
            {
                get
                {
                    Ensure.CanOperate(_index != 0 && _index != _dictionary._count + 1, "The dictionary has been modified during the iteration!");

                    if (_getEnumeratorRetType == DictEntry)
                        return new DictionaryEntry(_current.Key, _current.Value);
                    else
                        return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
                }
            }

            void IEnumerator.Reset()
            {
                Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                _index = 0;
                _current = default;
            }
        }

        [DebuggerDisplay("Count = {Count}")]
        public sealed class KeyCollection : ICollection<TKey>, IReadOnlyCollection<TKey>
        {
            private readonly Dictionary64<TKey, TValue> _dictionary;

            public KeyCollection(Dictionary64<TKey, TValue> dictionary)
            {
                Ensure.NotNull(_dictionary);
                _dictionary = dictionary;
            }

            public Enumerator GetEnumerator()
                => new Enumerator(_dictionary);

            public void CopyTo(TKey[] array, int index)
            {
                Ensure.NotNull(array);
                Ensure.NotOutOfRange((uint)index <= array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                var count = _dictionary._count;
                var entries = _dictionary._entries;

                foreach (var entry in entries)
                {
                    if (entry.next >= -1) 
                        array[index++] = entry.key;
                }
            }

            public int Count 
                => _dictionary.Count;

            bool ICollection<TKey>.IsReadOnly 
                => true;

            void ICollection<TKey>.Add(TKey item)
                => throw new NotSupportedException();

            void ICollection<TKey>.Clear()
                => throw new NotSupportedException();

            bool ICollection<TKey>.Contains(TKey item)
                => _dictionary.ContainsKey(item);

            bool ICollection<TKey>.Remove(TKey item)
                => throw new NotSupportedException();

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
                => new Enumerator(_dictionary);

            IEnumerator IEnumerable.GetEnumerator()
                => new Enumerator(_dictionary);

            public struct Enumerator : IEnumerator<TKey>
            {
                private readonly Dictionary64<TKey, TValue> _dictionary;
                private int _index;
                private readonly int _version;
                [AllowNull, MaybeNull] private TKey _currentKey;

                internal Enumerator(Dictionary64<TKey, TValue> dictionary)
                {
                    _dictionary = dictionary;
                    _version = dictionary._version;
                    _index = 0;
                    _currentKey = default;
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                    while ((uint)_index < (uint)_dictionary._count)
                    {
                        ref var entry = ref _dictionary._entries![_index++];

                        if (entry.next >= -1)
                        {
                            _currentKey = entry.key;
                            return true;
                        }
                    }

                    _index = _dictionary._count + 1;
                    _currentKey = default;
                    return false;
                }

                public TKey Current => _currentKey!;

                object? IEnumerator.Current
                {
                    get
                    {
                        Ensure.CanOperate(_index != 0 && _index != _dictionary._count + 1, "The dictionary has been modified during the iteration!");
                        return _currentKey;
                    }
                }

                void IEnumerator.Reset()
                {
                    Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                    _index = 0;
                    _currentKey = default;
                }
            }
        }

        [DebuggerDisplay("Count = {Count}")]
        public sealed class ValueCollection : ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            private readonly Dictionary64<TKey, TValue> _dictionary;

            public ValueCollection(Dictionary64<TKey, TValue> dictionary)
            {
                Ensure.NotNull(dictionary);
                _dictionary = dictionary;
            }

            public Enumerator GetEnumerator()
                => new Enumerator(_dictionary);

            public void CopyTo(TValue[] array, int index)
            {
                Ensure.NotNull(array);
                Ensure.NotOutOfRange((uint)index <= array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                var count = _dictionary._count;
                var entries = _dictionary._entries;

                foreach (var entry in entries)
                {
                    if (entry.next >= -1)
                        array[index++] = entry.value;
                }
            }

            public int Count 
                => _dictionary.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item)
                => throw new NotSupportedException();

            bool ICollection<TValue>.Remove(TValue item)
                => throw new NotSupportedException();

            void ICollection<TValue>.Clear()
                => throw new NotSupportedException();

            bool ICollection<TValue>.Contains(TValue item)
                => _dictionary.ContainsValue(item);

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
                => new Enumerator(_dictionary);

            IEnumerator IEnumerable.GetEnumerator()
                => new Enumerator(_dictionary);

            public struct Enumerator : IEnumerator<TValue>
            {
                private readonly Dictionary64<TKey, TValue> _dictionary;
                private int _index;
                private readonly int _version;
                [AllowNull, MaybeNull] private TValue _currentValue;

                internal Enumerator(Dictionary64<TKey, TValue> dictionary)
                {
                    _dictionary = dictionary;
                    _version = dictionary._version;
                    _index = 0;
                    _currentValue = default;
                }

                public void Dispose()
                {
                }

                public bool MoveNext()
                {
                    Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                    while ((uint)_index < (uint)_dictionary._count)
                    {
                        ref var entry = ref _dictionary._entries![_index++];

                        if (entry.next >= -1)
                        {
                            _currentValue = entry.value;
                            return true;
                        }
                    }
                    _index = _dictionary._count + 1;
                    _currentValue = default;
                    return false;
                }

                public TValue Current 
                    => _currentValue!;

                object? IEnumerator.Current
                {
                    get
                    {
                        Ensure.CanOperate(_index != 0 && _index != _dictionary._count + 1, "The dictionary has been modified during the iteration!");
                        return _currentValue;
                    }
                }

                void IEnumerator.Reset()
                {
                    Ensure.CanOperate(_version == _dictionary._version, "The dictionary has been modified during the iteration!");

                    _index = 0;
                    _currentValue = default;
                }
            }
        }
    }
}

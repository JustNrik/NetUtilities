using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NetUtilities;

namespace System.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public class Dictionary64<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        private enum InsertionBehavior : byte
        {
            None = 0,
            OverwriteExisting = 1,
            ThrowOnExisting = 2
        }

        private struct Entry
        {
            public ulong HashCode;
            public int Next;
            public TKey Key;
            public TValue Value;
        }

        private const int StartOfFreeList = -3;

        private static readonly int[] _primes =
        {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
        };

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

        public IEqualityComparer64<TKey> Comparer
            => _comparer is null
            ? EqualityComparer64<TKey>.Default
            : _comparer;

        public int Count
            => _count - _freeCount;

        public KeyCollection Keys
            => _keys ??= new KeyCollection(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
            => Keys;

        public ValueCollection Values
            => _values ??= new ValueCollection(this);

        ICollection<TValue> IDictionary<TKey, TValue>.Values
            => Values;

        public TValue this[TKey key]
        {
            get
            {
                ref var value = ref FindValue(key);

                if (!Unsafe.IsNullRef(ref value))
                    return value;

                throw new KeyNotFoundException();
            }
            set => TryInsert(key, value, InsertionBehavior.OverwriteExisting);
        }

        public Dictionary64() : this(0, null)
        {
        }

        public Dictionary64(int capacity) : this(capacity, null)
        {
        }

        public Dictionary64(IEqualityComparer64<TKey>? comparer) : this(0, comparer)
        {
        }

        public Dictionary64(int capacity, IEqualityComparer64<TKey>? comparer)
        {
            Ensure.NotOutOfRange(capacity >= 0, capacity);

            if (capacity > 0)
                Initialize(capacity);

            if (comparer is not null && comparer != EqualityComparer64<TKey>.Default) // first check for null to avoid forcing default comparer instantiation unnecessarily
                _comparer = comparer;
        }

        public Dictionary64(IDictionary<TKey, TValue> dictionary) : this(dictionary, null)
        {
        }

        public Dictionary64(IDictionary<TKey, TValue> dictionary, IEqualityComparer64<TKey>? comparer) :
            this(dictionary?.Count ?? 0, comparer)
        {
            Ensure.NotNull(dictionary);

            if (dictionary.GetType() == typeof(Dictionary64<TKey, TValue>))
            {
                var d = (Dictionary64<TKey, TValue>)dictionary;
                var count = d._count;
                var entries = d._entries;

                foreach (var entry in entries)
                {
                    if (entry.Next >= -1)
                        Add(entry.Key, entry.Value);
                }
            }
            else
            {
                foreach (var pair in dictionary)
                    Add(pair.Key, pair.Value);
            }
        }

        public Dictionary64(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null)
        {
        }

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
            return !Unsafe.IsNullRef(ref value) && EqualityComparer64<TValue>.Default.Equals(value, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            ref TValue value = ref FindValue(keyValuePair.Key);

            if (!Unsafe.IsNullRef(ref value) && EqualityComparer64<TValue>.Default.Equals(value, keyValuePair.Value))
            {
                Remove(keyValuePair.Key);
                return true;
            }

            return false;
        }

        private static ulong GetHashCode64(TKey key, IEqualityComparer64<TKey>? comparer = null)
            => (ulong)(comparer?.GetHashCode64(key) ?? EqualityComparer64<TKey>.Default.GetHashCode64(key));

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
            => !Unsafe.IsNullRef(ref FindValue(key));

        public bool ContainsValue(TValue value)
        {
            var entries = _entries;
            var entry = default(Entry)!;

            if (value is null)
            {
                for (int i = 0; i < _count; i++)
                {
                    entry = entries![i];

                    if (entry.Next is >= -1 && entry.Value is not null)
                        return true;
                }
            }
            else if (typeof(TValue).IsValueType)
            {
                // ValueType: Devirtualize with EqualityComparer64<TValue>.Default intrinsic
                for (int i = 0; i < _count; i++)
                {
                    entry = entries![i];

                    if (entry.Next >= -1 && EqualityComparer64<TValue>.Default.Equals(entry.Value, value))
                        return true;
                }
            }
            else
            {
                // Object type: Shared Generic, EqualityComparer64<TValue>.Default won't devirtualize
                // https://github.com/dotnet/runtime/issues/10050
                // So cache in a local rather than get EqualityComparer per loop iteration
                var defaultComparer = EqualityComparer64<TValue>.Default;

                for (int i = 0; i < _count; i++)
                {
                    entry = entries![i];

                    if (entries![i].Next >= -1 && defaultComparer.Equals(entry.Value, value))
                        return true;
                }
            }

            return false;
        }

        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            Ensure.NotNull(array);
            Ensure.NotOutOfRange((uint)index <= (uint)array.Length, index);
            Ensure.NotOutOfRange(array.Length - index < Count, index);

            var count = _count;
            var entries = _entries;

            for (int i = 0; i < count; i++)
            {
                var entry = entries![i];

                if (entry.Next >= -1)
                    array[index++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }

        public Enumerator GetEnumerator()
            => new Enumerator(this, Enumerator.KeyValuePair);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
            new Enumerator(this, Enumerator.KeyValuePair);

        private ref TValue FindValue(TKey key)
        {
            Ensure.NotNull(key);

            ref var entry = ref Unsafe.NullRef<Entry>();

            if (_buckets is not null)
            {
                var comparer = _comparer;

                if (comparer is null)
                {
                    var hashCode = GetHashCode64(key);
                    var i = GetBucket(hashCode);
                    var entries = _entries;
                    var collisionCount = 0u;

                    if (typeof(TKey).IsValueType)
                    {
                        // ValueType: Devirtualize with EqualityComparer64<TValue>.Default intrinsic

                        i--;

                        do
                        {
                            if ((uint)i >= (uint)entries.Length)
                                goto ReturnNotFound;

                            entry = ref entry;

                            if (entry.HashCode == hashCode && EqualityComparer64<TKey>.Default.Equals(entry.Key, key))
                                goto ReturnFound;

                            i = entry.Next;

                            collisionCount++;
                        } while (collisionCount <= (uint)entries.Length);

                        goto ConcurrentOperation;
                    }
                    else
                    {
                        // Object type: Shared Generic, EqualityComparer64<TValue>.Default won't devirtualize
                        // https://github.com/dotnet/runtime/issues/10050var hashCode = (ulong)(comparer?.GetHashCode64(key) ?? EqualityComparer64<TKey>.Default.GetHashCode64(key));
                        // So cache in a local rather than get EqualityComparer per loop iteration
                        var defaultComparer = EqualityComparer64<TKey>.Default;

                        i--;

                        do
                        {
                            if ((uint)i >= (uint)entries.Length)
                                goto ReturnNotFound;

                            entry = ref entry;

                            if (entry.HashCode == hashCode && defaultComparer.Equals(entry.Key, key))
                                goto ReturnFound;

                            i = entry.Next;

                            collisionCount++;
                        } while (collisionCount <= (uint)entries.Length);

                        goto ConcurrentOperation;
                    }
                }
                else
                {
                    var hashCode = GetHashCode64(key);
                    var i = GetBucket(hashCode);
                    var entries = _entries;
                    var collisionCount = 0u;

                    i--;

                    do
                    {
                        if ((uint)i >= (uint)entries.Length)
                            goto ReturnNotFound;

                        entry = ref entry;

                        if (entry.HashCode == hashCode && comparer.Equals(entry.Key, key))
                            goto ReturnFound;

                        i = entry.Next;

                        collisionCount++;
                    } while (collisionCount <= (uint)entries.Length);

                    goto ConcurrentOperation;
                }
            }

            goto ReturnNotFound;

            ConcurrentOperation:
            Ensure.CanOperate(false, message: "Concurrent operations are not supposed.");
            ReturnFound:
            ref TValue value = ref entry.Value;
            Return:
            return ref value;
            ReturnNotFound:
            value = ref Unsafe.NullRef<TValue>();
            goto Return;
        }

        private static int GetPrime(int min)
        {
            Ensure.NotOutOfRange(min >= 0, min);

            foreach (int prime in _primes)
            {
                if (prime >= min)
                    return prime;
            }

            // Outside of our predefined table. Compute the hard way.
            for (var i = (min | 1); i < int.MaxValue; i += 2)
                if (i.IsPrime() && ((i - 1) % 101 != 0))
                    return i;

            return min;
        }

        private int Initialize(int capacity)
        {
            var size = GetPrime(capacity);
            var buckets = new int[size];
            var entries = new Entry[size];

            _freeList = -1;
            _fastModMultiplier = ulong.MaxValue / (uint)size + 1;
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
            var hashCode = GetHashCode64(key);
            var collisionCount = 0u;
            ref var bucket = ref GetBucket(hashCode);
            var i = bucket - 1;
            var entry = default(Entry)!;

            if (comparer is null)
            {
                if (typeof(TKey).IsValueType)
                {
                    // ValueType: Devirtualize with EqualityComparer64<TValue>.Default intrinsic
                    while (true)
                    {
                        // Should be a while loop https://github.com/dotnet/runtime/issues/9422
                        // Test uint in if rather than loop condition to drop range check for following array access
                        if ((uint)i >= (uint)entries.Length)
                            break;

                        entry = entries[i];

                        if (entry.HashCode == hashCode && EqualityComparer64<TKey>.Default.Equals(entry.Key, key))
                        {
                            Ensure.CanOperate(behavior != InsertionBehavior.ThrowOnExisting, message: "The key already exists.");

                            if (behavior == InsertionBehavior.OverwriteExisting)
                            {
                                entry.Value = value;
                                return true;
                            }

                            return false;
                        }

                        i = entry.Next;

                        collisionCount++;
                        Ensure.CanOperate(collisionCount <= (uint)entries.Length, message: "Concurrent operations are not supported.");
                    }
                }
                else
                {
                    // Object type: Shared Generic, EqualityComparer64<TValue>.Default won't devirtualize
                    // https://github.com/dotnet/runtime/issues/10050
                    // So cache in a local rather than get EqualityComparer per loop iteration
                    var defaultComparer = EqualityComparer64<TKey>.Default;

                    while (true)
                    {
                        // Should be a while loop https://github.com/dotnet/runtime/issues/9422
                        // Test uint in if rather than loop condition to drop range check for following array access
                        if ((uint)i >= (uint)entries.Length)
                            break;

                        entry = entries[i];

                        if (entry.HashCode == hashCode && defaultComparer.Equals(entry.Key, key))
                        {
                            Ensure.CanOperate(behavior != InsertionBehavior.ThrowOnExisting, message: "The key already exists.");

                            if (behavior == InsertionBehavior.OverwriteExisting)
                            {
                                entry.Value = value;
                                return true;
                            }

                            return false;
                        }

                        i = entry.Next;

                        collisionCount++;
                        Ensure.CanOperate(collisionCount <= (uint)entries.Length, message: "Concurrent operations are not supported.");
                    }
                }
            }
            else
            {
                while (true)
                {
                    // Should be a while loop https://github.com/dotnet/runtime/issues/9422
                    // Test uint in if rather than loop condition to drop range check for following array access
                    if ((uint)i >= (uint)entries.Length)
                        break;

                    entry = entries[i];

                    if (entry.HashCode == hashCode && comparer.Equals(entry.Key, key))
                    {
                        Ensure.CanOperate(behavior != InsertionBehavior.ThrowOnExisting, message: "The key already exists.");

                        if (behavior == InsertionBehavior.OverwriteExisting)
                        {
                            entry.Value = value;
                            return true;
                        }

                        return false;
                    }

                    i = entry.Next;

                    collisionCount++;
                    Ensure.CanOperate(collisionCount <= (uint)entries.Length, message: "Concurrent operations are not supported.");
                }
            }

            int index;

            if (_freeCount > 0)
            {
                index = _freeList;
                _freeList = StartOfFreeList - entries[_freeList].Next;
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

            ref var e = ref entries![index];
            e.HashCode = hashCode;
            e.Next = bucket - 1; // Value in _buckets is 1-based
            e.Key = key;
            e.Value = value; // Value in _buckets is 1-based
            bucket = index + 1;
            _version++;

            // Value types never rehash
            if (!typeof(TKey).IsValueType && collisionCount > 100)
            {
                // If we hit the collision threshold we'll need to switch to the comparer which is using randomized string hashing
                // i.e. EqualityComparer64<string>.Default.
                _comparer = null;
                Resize(entries.Length, true);
            }

            return true;
        }

        private static int ExpandPrime(int oldSize)
        {
            const int MaxPrimeArrayLength = 0x7FEFFFFD;

            var newSize = 2 * oldSize;

            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
                return MaxPrimeArrayLength;

            return GetPrime(newSize);
        }

        private void Resize()
            => Resize(ExpandPrime(_count), false);

        private void Resize(int newSize, bool forceNewHashCodes)
        {
            var entries = new Entry[newSize];
            var count = _count;

            Array.Copy(_entries, entries, count);

            if (!typeof(TKey).IsValueType && forceNewHashCodes)
            {
                for (int i = 0; i < count; i++)
                {
                    var entry = entries![i];

                    if (entry.Next >= -1)
                        entry.HashCode = (ulong)EqualityComparer64<TKey>.Default.GetHashCode64(entry.Key);
                }
            }

            _buckets = new int[newSize];
            _fastModMultiplier = ulong.MaxValue / (uint)newSize + 1;

            for (int i = 0; i < count; i++)
            {
                var entry = entries![i];

                if (entry.Next >= -1)
                {
                    ref var bucket = ref GetBucket(entry.HashCode);
                    entry.Next = bucket - 1;
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
                var hashCode = GetHashCode64(key);
                ref var bucket = ref GetBucket(hashCode);
                var entries = _entries;
                var last = -1;
                var i = bucket - 1;

                while (i >= 0)
                {
                    ref var entry = ref entries[i];

                    if (entry.HashCode == hashCode && (_comparer?.Equals(entry.Key, key) ?? EqualityComparer64<TKey>.Default.Equals(entry.Key, key)))
                    {
                        if (last < 0)
                            bucket = entry.Next + 1; // Value in buckets is 1-based
                        else
                            entries[last].Next = entry.Next;

                        entry.Next = StartOfFreeList - _freeList;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                            entry.Key = default!;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                            entry.Value = default!;

                        _freeList = i;
                        _freeCount++;
                        return true;
                    }

                    last = i;
                    i = entry.Next;

                    collisionCount++;
                    Ensure.CanOperate(collisionCount <= (uint)entries.Length, "Concurrent operations are not supported.");
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
                var hashCode = GetHashCode64(key);
                ref var bucket = ref GetBucket(hashCode);
                var entries = _entries;
                var last = -1;
                var i = bucket - 1;

                while (i >= 0)
                {
                    ref var entry = ref entries![i];

                    if (entry.HashCode == hashCode && (_comparer?.Equals(entry.Key, key) ?? EqualityComparer64<TKey>.Default.Equals(entry.Key, key)))
                    {
                        if (last < 0)
                            bucket = entry.Next + 1;
                        else
                            entries[last].Next = entry.Next;

                        value = entry.Value;

                        entry.Next = StartOfFreeList - _freeList;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                            entry.Key = default!;

                        if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                            entry.Value = default!;

                        _freeList = i;
                        _freeCount++;
                        return true;
                    }

                    last = i;
                    i = entry.Next;

                    collisionCount++;
                    Ensure.CanOperate(collisionCount <= (uint)entries.Length, "Concurrent operations are not supported.");
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            ref var valRef = ref FindValue(key);

            if (!Unsafe.IsNullRef(ref valRef))
            {
                value = valRef;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryAdd(TKey key, TValue value)
            => TryInsert(key, value, InsertionBehavior.None);

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
            => false;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
            => CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this, Enumerator.KeyValuePair);

        /// <summary>
        ///     Ensures that the dictionary can hold up to 'capacity' entries without any further expansion of its backing storage
        /// </summary>
        public int EnsureCapacity(int capacity)
        {
            Ensure.NotOutOfRange(capacity >= 0, capacity);

            var currentCapacity = _entries?.Length ?? 0;

            if (currentCapacity >= capacity)
                return currentCapacity;

            _version++;

            if (_buckets is null)
                return Initialize(capacity);

            var newSize = GetPrime(capacity);

            Resize(newSize, false);
            return newSize;
        }

        /// <summary>
        /// Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its entries
        /// </summary>
        /// <remarks>
        /// This method can be used to minimize the memory overhead
        /// once it is known that no new elements will be added.
        ///
        /// To allocate minimum size storage array, execute the following statements:
        ///
        /// dictionary.Clear();
        /// dictionary.TrimExcess();
        /// </remarks>
        public void TrimExcess()
            => TrimExcess(Count);

        /// <summary>
        /// Sets the capacity of this dictionary to hold up 'capacity' entries without any further expansion of its backing storage
        /// </summary>
        /// <remarks>
        /// This method can be used to minimize the memory overhead
        /// once it is known that no new elements will be added.
        /// </remarks>
        public void TrimExcess(int capacity)
        {
            Ensure.NotOutOfRange(capacity >= Count, capacity);

            var newSize = GetPrime(capacity);
            var oldEntries = _entries;
            var currentCapacity = oldEntries?.Length ?? 0;

            if (newSize >= currentCapacity)
                return;

            var oldCount = _count;

            _version++;
            Initialize(newSize);

            var entries = _entries;
            var count = 0;

            for (var i = 0; i < oldCount; i++)
            {
                var oldEntry = oldEntries![i];
                var hashCode = oldEntry.HashCode; // At this point, we know we have entries.
                if (oldEntry.Next >= -1)
                {
                    ref var entry = ref entries![count];
                    entry = oldEntries[i];
                    ref var bucket = ref GetBucket(hashCode);
                    entry.Next = bucket - 1;
                    bucket = count + 1;
                    count++;
                }
            }

            _count = count;
            _freeCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref int GetBucket(ulong hashCode)
        {
            var buckets = _buckets!;
            return ref buckets[FastMod(hashCode, (uint)buckets.Length, _fastModMultiplier)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FastMod(ulong value, uint divisor, ulong multiplier)
            => (uint)(((((multiplier * value) >> 32) + 1) * divisor) >> 32);

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
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
                Ensure.CanOperate(_version == _dictionary._version);

                while ((uint)_index < (uint)_dictionary._count)
                {
                    ref var entry = ref _dictionary._entries![_index++];

                    if (entry.Next >= -1)
                    {
                        _current = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                        return true;
                    }
                }

                _index = _dictionary._count + 1;
                _current = default;
                return false;
            }

            public KeyValuePair<TKey, TValue> Current
                => _current;

            public void Dispose()
            {
            }

            object? IEnumerator.Current
            {
                get
                {
                    Ensure.CanOperate(_index != 0 && (_index != _dictionary._count + 1));

                    if (_getEnumeratorRetType == DictEntry)
                        return new DictionaryEntry(_current.Key, _current.Value);

                    return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
                }
            }

            void IEnumerator.Reset()
            {
                Ensure.CanOperate(_version == _dictionary._version);

                _index = 0;
                _current = default;
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    Ensure.CanOperate(_index > 0 && _index <= _dictionary._count);
                    return new DictionaryEntry(_current.Key, _current.Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    Ensure.CanOperate(_index > 0 && _index <= _dictionary._count);
                    return _current.Key;
                }
            }

            object? IDictionaryEnumerator.Value
            {
                get
                {
                    Ensure.CanOperate(_index > 0 && _index <= _dictionary._count);
                    return _current.Value;
                }
            }
        }

        [DebuggerDisplay("Count = {Count}")]
        public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey>
        {
            private readonly Dictionary64<TKey, TValue> _dictionary;

            public KeyCollection(Dictionary64<TKey, TValue> dictionary)
            {
                _dictionary = Ensure.NotNull(dictionary);
            }

            public Enumerator GetEnumerator()
                => new Enumerator(_dictionary);

            public void CopyTo(TKey[] array, int index)
            {
                Ensure.NotNull(array);
                Ensure.NotOutOfRange(index >= 0 && index <= array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                var count = _dictionary._count;
                var entries = _dictionary._entries;

                for (int i = 0; i < count; i++)
                {
                    var entry = entries![i];

                    if (entry.Next >= -1)
                        array[index++] = entry.Key;
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

            void ICollection.CopyTo(Array array, int index)
            {
                Ensure.NotNull(array);
                Ensure.CanOperate(array.Rank == 1);
                Ensure.CanOperate(array.GetLowerBound(0) == 0);
                Ensure.NotOutOfRange((uint)index <= (uint)array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                if (array is TKey[] keys)
                    CopyTo(keys, index);
                else
                {
                    var objects = array as object[];
                    var count = _dictionary._count;
                    var entries = _dictionary._entries;

                    Ensure.CanOperate(objects is not null, message: "The input array has a wrong type.");

                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var entry = entries![i];

                            if (entry.Next >= -1)
                                objects[index++] = entry.Key;
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        Ensure.CanOperate(false, message: "The input array has a wrong type.");
                    }
                }
            }

            bool ICollection.IsSynchronized
                => false;

            object ICollection.SyncRoot
                => ((ICollection)_dictionary).SyncRoot;

            public struct Enumerator : IEnumerator<TKey>, IEnumerator
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
                    Ensure.CanOperate(_version == _dictionary._version);

                    while ((uint)_index < (uint)_dictionary._count)
                    {
                        ref var entry = ref _dictionary._entries![_index++];

                        if (entry.Next >= -1)
                        {
                            _currentKey = entry.Key;
                            return true;
                        }
                    }

                    _index = _dictionary._count + 1;
                    _currentKey = default;
                    return false;
                }

                public TKey Current
                    => _currentKey!;

                object? IEnumerator.Current
                {
                    get
                    {
                        Ensure.CanOperate(_index > 0 && _index <= _dictionary._count);
                        return _currentKey;
                    }
                }

                void IEnumerator.Reset()
                {
                    Ensure.CanOperate(_version == _dictionary._version);

                    _index = 0;
                    _currentKey = default;
                }
            }
        }

        [DebuggerDisplay("Count = {Count}")]
        public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue>
        {
            private readonly Dictionary64<TKey, TValue> _dictionary;

            public ValueCollection(Dictionary64<TKey, TValue> dictionary)
            {
                _dictionary = Ensure.NotNull(dictionary);
            }

            public Enumerator GetEnumerator()
                => new Enumerator(_dictionary);

            public void CopyTo(TValue[] array, int index)
            {
                Ensure.NotNull(array);
                Ensure.NotOutOfRange(index >= 0 && index <= array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                var count = _dictionary._count;
                var entries = _dictionary._entries;

                for (int i = 0; i < count; i++)
                {
                    var entry = entries![i];

                    if (entry.Next >= -1)
                        array[index++] = entry.Value;
                }
            }

            public int Count
                => _dictionary.Count;

            bool ICollection<TValue>.IsReadOnly
                => true;

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

            void ICollection.CopyTo(Array array, int index)
            {
                Ensure.NotNull(array);
                Ensure.CanOperate(array.Rank == 1);
                Ensure.CanOperate(array.GetLowerBound(0) == 0);
                Ensure.NotOutOfRange((uint)index <= (uint)array.Length, index);
                Ensure.NotOutOfRange(array.Length - index >= _dictionary.Count, index);

                if (array is TValue[] values)
                    CopyTo(values, index);
                else
                {
                    var objects = array as object[];
                    var count = _dictionary._count;
                    var entries = _dictionary._entries;

                    Ensure.CanOperate(objects is not null, message: "The input array has a wrong type.");

                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var entry = entries![i];

                            if (entry.Next >= -1)
                                objects[index++] = entry.Value;
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        Ensure.CanOperate(false, message: "The input array has a wrong type.");
                    }
                }
            }

            bool ICollection.IsSynchronized
                => false;

            object ICollection.SyncRoot
                => ((ICollection)_dictionary).SyncRoot;

            public struct Enumerator : IEnumerator<TValue>, IEnumerator
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

                public void Dispose() { }

                public bool MoveNext()
                {
                    Ensure.CanOperate(_version == _dictionary._version);

                    while ((uint)_index < (uint)_dictionary._count)
                    {
                        ref var entry = ref _dictionary._entries![_index++];

                        if (entry.Next >= -1)
                        {
                            _currentValue = entry.Value;
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
                        Ensure.CanOperate(_index > 0 && _index <= _dictionary._count);
                        return _currentValue;
                    }
                }

                void IEnumerator.Reset()
                {
                    Ensure.CanOperate(_version == _dictionary._version);

                    _index = 0;
                    _currentValue = default;
                }
            }
        }
    }
}

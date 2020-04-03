using NetUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Collections.Generic
{
    public sealed class Lookup<TKey, TValue> : ILookup<TKey, TValue>, IDictionary<TKey, List<TValue>>
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;
        private readonly IDictionary<TKey, List<TValue>> _lookup;

        public Lookup()
        {
            _lookup = new Dictionary<TKey, List<TValue>>();
        }

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] 
            => _lookup[key];

        public List<TValue> this[TKey key] 
        { 
            [MethodImplementation(Inlined)]
            get => _lookup[key];
            [MethodImplementation(Inlined)]
            set => _lookup[key] = value;
        }

        public int Count 
            => _lookup.Count;

        public ICollection<TKey> Keys 
            => _lookup.Keys;

        public ICollection<List<TValue>> Values 
            => _lookup.Values;

        public bool IsReadOnly 
            => false;

        public void Add(TKey key, List<TValue> value)
        {
            if (key is null)
                Throw.NullArgument(nameof(key));

            _lookup.Add(key, value);
        }

        public void Add(TKey key, TValue value)
        {
            if (key is null)
                Throw.NullArgument(nameof(key));

            if (!_lookup.TryGetValue(key, out var list))
                _lookup[key] = new List<TValue>() { value };
            else
                list.Add(value);
        }

        [MethodImplementation(Inlined)]
        public void Add(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Add(item);

        [MethodImplementation(Inlined)]
        public void Clear()
            => _lookup.Clear();

        [MethodImplementation(Inlined)]
        public bool Contains(TKey key)
            => _lookup.ContainsKey(key);

        [MethodImplementation(Inlined)]
        public bool Contains(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Contains(item);

        [MethodImplementation(Inlined)]
        public bool ContainsKey(TKey key)
            => _lookup.ContainsKey(key);

        [MethodImplementation(Inlined)]
        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
            => _lookup.CopyTo(array, arrayIndex);

        [MethodImplementation(Inlined)]
        public LookupEnumerator GetEnumerator()
            => new LookupEnumerator(_lookup);

        [MethodImplementation(Inlined)]
        public bool Remove(TKey key)
            => _lookup.Remove(key);

        [MethodImplementation(Inlined)]
        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Remove(item);

        [MethodImplementation(Inlined)]
        public bool Remove(TKey key, TValue value)
            => _lookup.TryGetValue(key, out var list) && list.Remove(value);

        [MethodImplementation(Inlined)]
        public bool TryGetValue(TKey key, out List<TValue> value)
            => _lookup.TryGetValue(key, out value);

        [MethodImplementation(Inlined)]
        IEnumerator IEnumerable.GetEnumerator()
            => new LookupEnumerator(_lookup);

        [MethodImplementation(Inlined)]
        IEnumerator<KeyValuePair<TKey, List<TValue>>> IEnumerable<KeyValuePair<TKey, List<TValue>>>.GetEnumerator()
            => _lookup.GetEnumerator();

        [MethodImplementation(Inlined)]
        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator()
            => new LookupEnumerator(_lookup);

        public sealed class LookupEnumerator : IEnumerator<IGrouping<TKey, TValue>>
        {
            private readonly IDictionary<TKey, List<TValue>> _lookup;
            private readonly TKey[] _keys;
            private int _count;
            public Grouping Current { get; private set; }

            IGrouping<TKey, TValue> IEnumerator<IGrouping<TKey, TValue>>.Current 
                => Current;

            object IEnumerator.Current 
                => Current;

            public LookupEnumerator(IDictionary<TKey, List<TValue>> lookup)
            {
                _lookup = lookup;
                _count = -1;
                _keys = lookup.Keys.ToArray();
                Current = default;
            }

            public bool MoveNext()
            {
                if (++_count < _keys.Length)
                {
                    var key = _keys[_count];
                    Current = new Grouping(key, _lookup[key]);
                    return true;
                }

                return false;
            }

            void IEnumerator.Reset()
            {
            }

            void IDisposable.Dispose()
            {
            }
        }

        public readonly struct Grouping : IGrouping<TKey, TValue>
        {
            private readonly List<TValue> _values;

            public TKey Key { get; }

            public Grouping(TKey key, List<TValue> values)
            {
                Key = key;
                _values = values;
            }

            [MethodImplementation(Inlined)]
            public List<TValue>.Enumerator GetEnumerator()
                => _values.GetEnumerator();

            [MethodImplementation(Inlined)]
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
                => GetEnumerator();

            [MethodImplementation(Inlined)]
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}

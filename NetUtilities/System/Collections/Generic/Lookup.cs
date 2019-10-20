using NetUtilities;
using System.Linq;

namespace System.Collections.Generic
{
    public sealed class Lookup<TKey, TValue> : ILookup<TKey, TValue>, IDictionary<TKey, IList<TValue>>
    {
        private readonly IDictionary<TKey, IList<TValue>> _lookup;

        public Lookup()
        {
            _lookup = new Dictionary<TKey, IList<TValue>>();
        }

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] => _lookup[key];

        public IList<TValue> this[TKey key] 
        { 
            get => _lookup[key]; 
            set => _lookup[key] = value;
        }

        public int Count => _lookup.Count;

        public ICollection<TKey> Keys => _lookup.Keys;

        public ICollection<IList<TValue>> Values => _lookup.Values;

        public bool IsReadOnly => false;

        public void Add(TKey key, IList<TValue> value)
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

        public void Add(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Add(item);

        public void Clear()
            => _lookup.Clear();

        public bool Contains(TKey key)
            => _lookup.ContainsKey(key);

        public bool Contains(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Contains(item);

        public bool ContainsKey(TKey key)
            => _lookup.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
            => _lookup.CopyTo(array, arrayIndex);

        public LookupEnumerator GetEnumerator()
            => new LookupEnumerator(_lookup);

        public bool Remove(TKey key)
            => _lookup.Remove(key);

        public bool Remove(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Remove(item);

        public bool Remove(TKey key, TValue value)
        {
            if (_lookup.TryGetValue(key, out var list))
                return list.Remove(value);

            return false;
        }

        public bool TryGetValue(TKey key, out IList<TValue> value)
            => _lookup.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
            => new LookupEnumerator(_lookup);

        IEnumerator<KeyValuePair<TKey, IList<TValue>>> IEnumerable<KeyValuePair<TKey, IList<TValue>>>.GetEnumerator()
            => _lookup.GetEnumerator();

        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator()
            => new LookupEnumerator(_lookup);

        public struct LookupEnumerator : IEnumerator<IGrouping<TKey, TValue>>
        {
            private readonly IDictionary<TKey, IList<TValue>> _lookup;
            private readonly TKey[] _keys;
            private int _count;
            public Grouping Current { get; private set; }

            IGrouping<TKey, TValue> IEnumerator<IGrouping<TKey, TValue>>.Current => Current;

            object IEnumerator.Current => throw new NotImplementedException();

            public LookupEnumerator(IDictionary<TKey, IList<TValue>> lookup)
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
            private readonly IEnumerable<TValue> _values;

            public Grouping(TKey key, IEnumerable<TValue> values)
            {
                Key = key;
                _values = values;
            }

            public TKey Key { get; }

            public IEnumerator<TValue> GetEnumerator()
                => _values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}

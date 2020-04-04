using NetUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// This class is an implementation of an one-to-many dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class Lookup<TKey, TValue> : ILookup<TKey, TValue>, IDictionary<TKey, List<TValue>> 
        where TKey : notnull
    {
        private readonly IDictionary<TKey, List<TValue>> _lookup;

        /// <summary>
        /// Creates an empty <see cref="Lookup{TKey, TValue}"/>
        /// </summary>
        public Lookup()
        {
            _lookup = new Dictionary<TKey, List<TValue>>();
        }

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] 
            => _lookup[key];

        /// <summary>
        /// Gets or sets the list associated with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A list with all values that are pointed from that key.</returns>
        public List<TValue> this[TKey key] 
        { 
            get => _lookup[key];
            set => _lookup[key] = value;
        }

        /// <inheritdoc/>
        public int Count 
            => _lookup.Count;

        /// <inheritdoc/>
        public ICollection<TKey> Keys 
            => _lookup.Keys;

        /// <inheritdoc/>
        public ICollection<List<TValue>> Values 
            => _lookup.Values;

        /// <inheritdoc/>
        public bool IsReadOnly 
            => false;

        /// <inheritdoc/>
        public void Add(TKey key, List<TValue> value)
        {
            Ensure.NotNull(key);

            _lookup.Add(key, value);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="Lookup{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            Ensure.NotNull(key);

            if (_lookup.TryGetValue(key, out var list))
                list.Add(value);
            else
                _lookup[key] = new List<TValue>() { value };
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Add(item);

        /// <inheritdoc/>
        public void Clear()
            => _lookup.Clear();

        /// <inheritdoc/>
        public bool Contains(TKey key)
            => _lookup.ContainsKey(key);

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Contains(item);

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
            => _lookup.ContainsKey(key);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
            => _lookup.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public LookupEnumerator GetEnumerator()
            => new LookupEnumerator(_lookup);

        /// <inheritdoc/>
        public bool Remove(TKey key)
            => _lookup.Remove(key);

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
            => _lookup.Remove(item);

        /// <inheritdoc/>
        public bool Remove(TKey key, TValue value)
            => _lookup.TryGetValue(key, out var list) && list.Remove(value);

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out List<TValue> value)
            => _lookup.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
            => new LookupEnumerator(_lookup);

        IEnumerator<KeyValuePair<TKey, List<TValue>>> IEnumerable<KeyValuePair<TKey, List<TValue>>>.GetEnumerator()
            => _lookup.GetEnumerator();

        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator()
            => new LookupEnumerator(_lookup);

        /// <summary>
        /// Supports a simple iteration over a generic collection.
        /// </summary>
        public sealed class LookupEnumerator : IEnumerator<IGrouping<TKey, TValue>>
        {
            private readonly IDictionary<TKey, List<TValue>> _lookup;
            private readonly TKey[] _keys;
            private int _count;

            /// <inheritdoc/>
            public Grouping Current { get; private set; }

            IGrouping<TKey, TValue> IEnumerator<IGrouping<TKey, TValue>>.Current 
                => Current;

            object IEnumerator.Current 
                => Current;

            /// <summary>
            /// Creates an enumerator to iterate over a <paramref name="lookup"/>
            /// </summary>
            /// <param name="lookup"></param>
            public LookupEnumerator(IDictionary<TKey, List<TValue>> lookup)
            {
                _lookup = lookup;
                _count = -1;
                _keys = lookup.Keys.ToArray();
                Current = default;
            }

            /// <inheritdoc/>
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

        /// <summary>
        /// Represents a collection of objects that have a common key.
        /// </summary>
        public readonly struct Grouping : IGrouping<TKey, TValue>
        {
            private readonly List<TValue> _values;

            /// <inheritdoc/>
            public TKey Key { get; }

            /// <summary>
            /// Creates a grouping with the providen key and values.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="values">The values.</param>
            public Grouping(TKey key, List<TValue> values)
            {
                Key = key;
                _values = values;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="List{TValue}"/>
            /// </summary>
            /// <returns>An enumerator that iterates through the <see cref="List{TValue}"/></returns>
            public List<TValue>.Enumerator GetEnumerator()
                => _values.GetEnumerator();

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
                => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}

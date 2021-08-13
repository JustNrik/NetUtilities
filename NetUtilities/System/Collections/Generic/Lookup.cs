using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Collections.Generic
{
    /// <summary>
    ///     This class is an implementation of an one-to-many dictionary.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The key type.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The value type.
    /// </typeparam>
    public sealed class Lookup<TKey, TValue> : ILookup<TKey, TValue>, IDictionary<TKey, IList<TValue>>
        where TKey : notnull
    {
        private readonly IDictionary<TKey, IList<TValue>> _lookup;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup{TKey, TValue}"/> <see langword="class"/>.
        /// </summary>
        public Lookup()
        {
            _lookup = new Dictionary<TKey, IList<TValue>>();
        }

        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key]
            => _lookup[key];

        /// <inheritdoc/>
        public IList<TValue> this[TKey key]
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
        public ICollection<IList<TValue>> Values
            => _lookup.Values;

        /// <inheritdoc/>
        public bool IsReadOnly
            => false;

        /// <inheritdoc/>
        public void Add(TKey key, IList<TValue> value)
        {
            Ensure.NotNull(key);

            _lookup.Add(key, value);
        }

        /// <summary>
        ///     Adds an element with the provided key and value to the <see cref="Lookup{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public void Add(TKey key, TValue value)
        {
            Ensure.NotNull(key);

            if (_lookup.TryGetValue(key, out var list))
                list.Add(value);
            else
                _lookup[key] = new List<TValue>() { value };
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Add(item);

        /// <inheritdoc/>
        public void Clear()
            => _lookup.Clear();

        /// <inheritdoc/>
        public bool Contains(TKey key)
            => _lookup.ContainsKey(key);

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Contains(item);

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
            => _lookup.ContainsKey(key);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
            => _lookup.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public LookupEnumerator GetEnumerator()
            => new(_lookup);

        /// <inheritdoc/>
        public bool Remove(TKey key)
            => _lookup.Remove(key);

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, IList<TValue>> item)
            => _lookup.Remove(item);

        /// <inheritdoc/>
        public bool Remove(TKey key, TValue value)
            => _lookup.TryGetValue(key, out var list) && list.Remove(value);

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out IList<TValue> value)
            => _lookup.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
            => new LookupEnumerator(_lookup);

        IEnumerator<KeyValuePair<TKey, IList<TValue>>> IEnumerable<KeyValuePair<TKey, IList<TValue>>>.GetEnumerator()
            => _lookup.GetEnumerator();

        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator()
            => new LookupEnumerator(_lookup);

        /// <inheritdoc cref="IEnumerator{T}"/>
        public sealed class LookupEnumerator : IEnumerator<IGrouping<TKey, TValue>>
        {
            private readonly IDictionary<TKey, IList<TValue>> _lookup;
            private readonly TKey[] _keys;
            private int _count;

            /// <inheritdoc/>
            public Grouping Current { get; private set; }

            IGrouping<TKey, TValue> IEnumerator<IGrouping<TKey, TValue>>.Current
                => Current;

            object IEnumerator.Current
                => Current;

            /// <summary>
            ///     Initializes a new instance of the <see cref="LookupEnumerator"/> <see langword="struct"/> 
            ///     with the provided lookup.
            /// </summary>
            public LookupEnumerator(IDictionary<TKey, IList<TValue>> lookup)
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

        /// <inheritdoc cref="IGrouping{TKey, TElement}"/>
        public readonly struct Grouping : IGrouping<TKey, TValue>
        {
            private readonly IList<TValue> _values;

            /// <inheritdoc/>
            public TKey Key { get; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="IGrouping{TKey, TElement}"/> <see langword="struct"/> 
            ///     with the provided keys and values.
            /// </summary>
            /// <param name="key">
            ///     The key.
            /// </param>
            /// <param name="values">
            ///     The values.
            /// </param>
            public Grouping(TKey key, IList<TValue> values)
            {
                Key = key;
                _values = values;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="IList{TValue}"/>
            /// </summary>
            /// <returns>An enumerator that iterates through the <see cref="IList{TValue}"/></returns>
            public IEnumerator<TValue> GetEnumerator()
                => _values.GetEnumerator();

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
                => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}

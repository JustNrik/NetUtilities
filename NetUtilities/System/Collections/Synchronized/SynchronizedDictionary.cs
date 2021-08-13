using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Collections.Synchronized
{
    /// <summary>
    ///     Provides a synchronized implementation of <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    public sealed class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICloneable<SynchronizedDictionary<TKey, TValue>>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            [return: MaybeNull]
            get
            {
                lock (_dictionary)
                    return _dictionary[key];
            }
            set
            {
                lock (_dictionary)
                    _dictionary[key] = value;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (_dictionary)
                    return _dictionary.Keys.ToHashSet();
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                lock (_dictionary)
                    return _dictionary.Values.ToHashSet();
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                lock (_dictionary)
                    return _dictionary.Count;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
            => false;

        /// <summary>
        ///     Initializes a new instace of the <see cref="SynchronizedDictionary{TKey, TValue}"/> <see langword="class"/> that is empty.
        /// </summary>
        public SynchronizedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        ///     Initializes a new instace of the <see cref="SynchronizedDictionary{TKey, TValue}"/> <see langword="class"/> that is empty,
        ///     and references the provided dictionary.
        /// </summary>
        /// <param name="dictionary">
        ///     The dictionary to reference.
        /// </param>
        public SynchronizedDictionary(Dictionary<TKey, TValue> dictionary) : this(dictionary, true)
        {
        }

        /// <summary>
        ///     Initializes a new instace of the <see cref="SynchronizedDictionary{TKey, TValue}"/> <see langword="class"/> that is empty,
        ///     and optionally you can indicate if you want to reference the provided dictionary.
        /// </summary>
        /// <param name="dictionary">
        ///     The dictionary.
        /// </param>
        /// <param name="keepReference">
        ///     Indicates if the reference to the provided dictionary should be kept.
        /// </param>
        public SynchronizedDictionary(Dictionary<TKey, TValue> dictionary, bool keepReference)
        {
            Ensure.NotNull(dictionary);

            _dictionary = keepReference
                ? dictionary
                : dictionary.ToDictionary();
        }

        /// <summary>
        ///     Initializes a new instace of the <see cref="SynchronizedDictionary{TKey, TValue}"/> <see langword="class"/> that is empty,
        ///     and contains the elements of the provided source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        public SynchronizedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            _dictionary = Ensure.NotNull(source).ToDictionary();
        }

        /// <summary>
        ///     Initializes a new instace of the <see cref="SynchronizedDictionary{TKey, TValue}"/> <see langword="class"/> that is empty,
        ///     and contains the elements of the provided source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        public SynchronizedDictionary(IEnumerable<(TKey, TValue)> source)
        {
            _dictionary = Ensure.NotNull(source).ToDictionary();
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            lock (_dictionary)
                _dictionary.Add(key, value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            lock (_dictionary)
                _dictionary.Add(pair.Key, pair.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            lock (_dictionary)
                _dictionary.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
                return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            lock (_dictionary)
                return _dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_dictionary)
                ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public Enumerator GetEnumerator()
        {
            lock (_dictionary)
                return new Enumerator(Clone());
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            lock (_dictionary)
                return _dictionary.Remove(key);
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
                return ((IDictionary<TKey, TValue>)_dictionary).Remove(item);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            lock (_dictionary)
                return _dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public SynchronizedDictionary<TKey, TValue> Clone()
        {
            lock (_dictionary)
                return new SynchronizedDictionary<TKey, TValue>(_dictionary, false);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        ///     Enumerates the elements of a <see cref="SynchronizedDictionary{TKey, TValue}"/>.
        /// </summary>
        public readonly struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly Dictionary<TKey, TValue>.Enumerator _enumerator;

            /// <inheritdoc/>
            public KeyValuePair<TKey, TValue> Current
                => _enumerator.Current;

            /// <summary>
            ///     Initializes a new instance of the <see cref="SynchronizedDictionary{TKey, TValue}.Enumerator"/> <see langword="struct"/> 
            ///     with the provided <see cref="SynchronizedDictionary{TKey, TValue}"/>.
            /// </summary>
            /// <param name="dictionary">
            ///     The source dictionary.
            /// </param>
            public Enumerator(SynchronizedDictionary<TKey, TValue> dictionary)
            {
                _enumerator = dictionary._dictionary.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
                => _enumerator.Dispose();

            /// <inheritdoc/>
            public bool MoveNext()
                => _enumerator.MoveNext();

            object? IEnumerator.Current
                => _enumerator.Current;

            void IEnumerator.Reset()
            {
            }
        }
    }
}

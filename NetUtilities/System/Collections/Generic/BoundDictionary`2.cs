using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Collections.Generic
{
    /// <summary>
    ///     Represents a generic collection of key/value pairs.
    ///     This collection only supports one-to-one relationship where the key is bound to the value.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The key type.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The value type.
    /// </typeparam>
    public sealed class BoundDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly IEqualityComparer<TKey>? _comparer;
        private unsafe readonly delegate*<TValue, TKey> _keySelector;

        public TValue this[TKey key] 
        {
            get => _dictionary[key]; 
            set
            {
                unsafe
                {
                    Ensure.NotNull(key);
                    Ensure.NotNull(value);

                    var valueKey = _keySelector(value);
                    var comparer = _comparer ?? EqualityComparer<TKey>.Default;

                    if (!comparer.Equals(key, valueKey))
                        throw new InvalidOperationException("The key doesn't match the value's key");

                    _dictionary.Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys 
            => _dictionary.Keys;

        public ICollection<TValue> Values 
            => _dictionary.Values;

        public int Count 
            => _dictionary.Count;

        public bool IsReadOnly 
            => _dictionary.IsReadOnly;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys 
            => _dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values 
            => _dictionary.Values;

        public BoundDictionary(Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>();
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(int capacity, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(capacity);
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(IDictionary<TKey, TValue> dictionary, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(dictionary);
            Ensure.NotNull(keySelector);

            _dictionary = dictionary;
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(IEqualityComparer<TKey>? comparer, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(comparer);
            _comparer = comparer;
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(int capacity, IEqualityComparer<TKey>? comparer, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
            _comparer = comparer;
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
            _comparer = comparer;
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(dictionary);
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public BoundDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary, IEqualityComparer<TKey>? comparer, Func<TValue, TKey> keySelector)
        {
            Ensure.NotNull(keySelector);

            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
            _comparer = comparer;
            unsafe
            {
                _keySelector = (delegate*<TValue, TKey>)keySelector.Method.MethodHandle.GetFunctionPointer();
            }
        }

        public void Add(TValue value)
        {
            unsafe
            {
                Ensure.NotNull(value);

                var key = _keySelector(value);
                _dictionary.Add(key, value);
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            unsafe
            {
                Ensure.NotNull(key);
                Ensure.NotNull(value);

                var valueKey = _keySelector(value);
                var comparer = _comparer ?? EqualityComparer<TKey>.Default;

                if (!comparer.Equals(key, valueKey))
                    throw new InvalidOperationException("The key doesn't match the value's key");

                _dictionary.Add(key, value);
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            unsafe
            {
                var (key, value) = item;
                
                Ensure.NotNull(key);
                Ensure.NotNull(value);

                var valueKey = _keySelector(value);
                var comparer = _comparer ?? EqualityComparer<TKey>.Default;

                if (!comparer.Equals(key, valueKey))
                    throw new InvalidOperationException("The key doesn't match the value's key");

                _dictionary.Add(key, value);
            }
        }

        public void Clear() 
            => _dictionary.Clear();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) 
            => _dictionary.Contains(item);

        public bool Contains(TValue value)
        {
            unsafe
            {
                Ensure.NotNull(value);
                return _dictionary.ContainsKey(_keySelector(value));
            }
        }

        public bool ContainsKey(TKey key) 
            => _dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) 
            => _dictionary.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() 
            => _dictionary.GetEnumerator();

        public bool Remove(TValue value)
        {
            unsafe
            {
                Ensure.NotNull(value);
                return _dictionary.Remove(_keySelector(value));
            }
        }

        public bool Remove(TKey key) 
            => _dictionary.Remove(key);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) 
            => _dictionary.Remove(item);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) 
            => _dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() 
            => ((IEnumerable)_dictionary).GetEnumerator();
        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => throw new NotImplementedException();
        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => throw new NotImplementedException();
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw new NotImplementedException();
    }
}

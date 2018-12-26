using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static void AddOrUpdate<T>(this IList<T> source, T element)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (element == null) throw new ArgumentNullException(nameof(element));

            var index = source.IndexOf(element);

            if (index == -1)
            {
                source.Add(element);
            }
            else
            {
                source[index] = element;
            }
        }

        private static readonly object _lock = new object();
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (source is IReadOnlyDictionary<TKey, TValue>) throw new InvalidOperationException($"{nameof(source)} is a readonly Dictionary");

            lock (_lock)
            {
                if (source.ContainsKey(key))
                {
                    source[key] = value;
                }
                else
                {
                    source.Add(key, value);
                }
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (source is IReadOnlyDictionary<TKey, TValue>) throw new InvalidOperationException($"{nameof(source)} is a readonly Dictionary");

            lock (_lock)
            {
                if (source.TryGetValue(key, out var val))
                {
                    return val;
                }
                else
                {
                    source.Add(key, value);
                    return value;
                }
            }
        }
    }
}

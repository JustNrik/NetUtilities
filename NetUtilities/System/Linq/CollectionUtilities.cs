using System.Collections.Generic;
using System.Collections.ObjectModel;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static void AddOrUpdate<T>(this IList<T> source, T element)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (element is null) throw new ArgumentNullException(nameof(element));

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

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (source is ReadOnlyDictionary<TKey, TValue>) throw new InvalidOperationException($"{nameof(source)} is a Read-Only Dictionary");

            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (key is null) throw new ArgumentNullException(nameof(key));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (source is ReadOnlyDictionary<TKey, TValue>) throw new InvalidOperationException($"{nameof(source)} is a Read-Only Dictionary");
            if (source.TryGetValue(key, out var val)) return val;

            source.Add(key, value);
            return value;
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.ToDictionary(x => x.Key, x => x.Value, comparer);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey key, TValue value)> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.ToDictionary(x => x.key, x => x.value);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey key, TValue value)> source, IEqualityComparer<TKey> comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.ToDictionary(x => x.key, x => x.value, comparer);
        }

        /// <summary>
        /// Creates a Read-Only Dictionary from the dictionary provided.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyDictionary<TKey, TValue>(source);
        }

        public static ReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyList<T>(source);
        }
    }
}

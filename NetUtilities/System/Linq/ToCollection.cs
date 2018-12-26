using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return new ReadOnlyDictionary<TKey, TValue>(source);
        }
    }
}

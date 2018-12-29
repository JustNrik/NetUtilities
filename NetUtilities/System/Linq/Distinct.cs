using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.Distinct(selector, null);
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return _(); IEnumerable<TSource> _()
            {
                var keys = new HashSet<TKey>(comparer);
                foreach (var item in source)
                {
                    if (keys.Add(selector(item)))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}

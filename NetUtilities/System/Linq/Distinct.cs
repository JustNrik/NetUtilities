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

            return DistinctIterator(source, selector, comparer);
        }

        private static IEnumerable<TSource> DistinctIterator<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer)
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

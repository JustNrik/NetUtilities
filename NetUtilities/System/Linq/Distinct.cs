using NetUtilities;
using System.Collections.Generic;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.Distinct(selector, null);
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey>? comparer)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(selector, nameof(selector));

            return DistinctIterator();

            IEnumerable<TSource> DistinctIterator()
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

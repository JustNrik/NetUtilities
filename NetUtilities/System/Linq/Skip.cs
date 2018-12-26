using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Skips all the elements while the predicate is True.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TSource> _()
            {
                using (var enumerator = source.GetEnumerator())
                {
                    while (enumerator.MoveNext() && predicate(enumerator.Current)) { } // Consuming the enumerable while the predicate is true;
                    while (enumerator.MoveNext()) yield return enumerator.Current;
                }
            }
        }

        /// <summary>
        /// Skips all the elements until the predicate is True
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TSource> _()
            {
                using (var enumerator = source.GetEnumerator())
                {
                    while (enumerator.MoveNext() && !predicate(enumerator.Current)) { } // Consuming the enumerable until the predicate is true;
                    yield return enumerator.Current;
                    while (enumerator.MoveNext()) yield return enumerator.Current;
                }
            }
        }
    }
}

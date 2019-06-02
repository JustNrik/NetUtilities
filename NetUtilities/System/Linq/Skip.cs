using NetUtilities;
using System.Collections.Generic;
#nullable enable
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
        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return SkipWhileIterator();

            IEnumerable<TSource> SkipWhileIterator()
            {
                using var e = source.GetEnumerator();
                while (e.MoveNext() && predicate(e.Current)) { } // Consuming the enumerable while the predicate is true;
                while (e.MoveNext()) yield return e.Current;
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
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return SkipUntilIterator();

            IEnumerable<TSource> SkipUntilIterator()
            {
                using var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext() && !predicate(enumerator.Current)) { } // Consuming the enumerable until the predicate is true;
                yield return enumerator.Current;
                while (enumerator.MoveNext()) yield return enumerator.Current;
            }
        }
    }
}

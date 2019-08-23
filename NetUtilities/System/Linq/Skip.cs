using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Skips all the elements while the predicate is True.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or predicate are null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="predicate">The delegated to filter the items.</param>
        /// <returns>A sequence of items</returns>
        [return: NotNull]
        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return SkipWhileIterator(source, predicate);
        }

        private static IEnumerable<TSource> SkipWhileIterator<TSource>(IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            using var e = source.GetEnumerator();
            while (e.MoveNext() && predicate(e.Current)) { } // Consuming the enumerable while the predicate is true
            while (e.MoveNext()) yield return e.Current;
        }

        /// <summary>
        /// Skips all the elements until the predicate is True.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or predicate are null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="predicate">The delegated to filter the items.</param>
        /// <returns>A sequence of items</returns>
        [return: NotNull]
        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return SkipUntilIterator(source, predicate);
        }

        private static IEnumerable<TSource> SkipUntilIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext() && !predicate(enumerator.Current)) { } // Consuming the enumerable until the predicate is true;
            yield return enumerator.Current;
            while (enumerator.MoveNext()) yield return enumerator.Current;
        }
    }
}

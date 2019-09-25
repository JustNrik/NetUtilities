using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        public static IEnumerable<TSource> SkipWhile<TSource>(
            [NotNull]this IEnumerable<TSource> source,
            [NotNull]Func<TSource, bool> predicate)
            => SkipWhileIterator(
                Ensure.NotNull(source, nameof(source)), 
                Ensure.NotNull(predicate, nameof(predicate)));

        private static IEnumerable<TSource> SkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!predicate(enumerator.Current))
                {
                    yield return enumerator.Current;

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;

                    break;
                }
            }
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
        public static IEnumerable<TSource> SkipUntil<TSource>(
            [NotNull]this IEnumerable<TSource> source,
            [NotNull]Func<TSource, bool> predicate)
            => SkipUntilIterator(
                Ensure.NotNull(source, nameof(source)), 
                Ensure.NotNull(predicate, nameof(predicate)));

        private static IEnumerable<TSource> SkipUntilIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext()) 
            {
                if (predicate(enumerator.Current))
                {
                    yield return enumerator.Current;

                    while (enumerator.MoveNext()) 
                        yield return enumerator.Current;

                    break;
                }
            }
        }
    }
}

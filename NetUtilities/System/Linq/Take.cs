using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Takes all the elements until the predicate is True.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or predicate are null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="predicate">The delegated to filter the items.</param>
        /// <returns>A sequence of items</returns>
        [return: NotNull]
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return TakeUntilIterator(source, predicate);
        }

        private static IEnumerable<TSource> TakeUntilIterator<TSource>(IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) yield break;
                yield return item;
            }
        }

        /// <summary>
        /// Takes all the elements while the predicate is True.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or predicate are null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="predicate">The delegated to filter the items.</param>
        /// <returns>A sequence of items</returns>
        [return: NotNull]
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return TakeWhileIterator(source, predicate);
        }

        private static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (!predicate(item)) yield break;

                yield return item;
            }
        }

        /// <summary>
        /// Takes an amount of items from the source that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either the source or the predicate are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when count is negative.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="count">The amount of items to be taken.</param>
        /// <param name="predicate">The delegate used for filtering.</param>
        /// <returns>A sequence with an amount equal or lower than the count give with the items that matches the predicate.</returns>
        [return: NotNull]
        public static IEnumerable<TSource> TakeIf<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));
            Ensure.Positive(count, nameof(count));

            return TakeIfIterator(source, count, predicate);
        }

        private static IEnumerable<TSource> TakeIfIterator<TSource>(IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;

                    if (--count == 0)
                        yield break;
                }
            }
        }
    }
}

using System.Collections.Generic;
using NetUtilities;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        ///     Takes all the elements until the predicate is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The underlying type of the sequence.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence.
        /// </param>
        /// <param name="predicate">
        ///     The delegated to filter the items.
        /// </param>
        /// <returns>
        ///     A sequence of items
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either source or predicate are <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            return TakeUntilIterator(source, predicate);
        }

        private static IEnumerable<TSource> TakeUntilIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) yield break;
                yield return item;
            }
        }

        /// <summary>
        ///     Takes all the elements while the predicate is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The underlying type of the sequence.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence.
        /// </param>
        /// <param name="predicate">
        ///     The delegated to filter the items.
        /// </param>
        /// <returns>
        ///     A sequence of items
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either source or predicate are <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

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
    }
}

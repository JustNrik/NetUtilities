using System.Collections.Generic;
using NetUtilities;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        ///     Skips all the elements while the predicate is <see langword="true"/>.
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
        public static IEnumerable<TSource> SkipWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
            => SkipWhileIterator(
                Ensure.NotNull(source),
                Ensure.NotNull(predicate));

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
        ///     Skips all the elements until the predicate is <see langword="true"/>.
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
        public static IEnumerable<TSource> SkipUntil<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
            => SkipUntilIterator(
                Ensure.NotNull(source),
                Ensure.NotNull(predicate));

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

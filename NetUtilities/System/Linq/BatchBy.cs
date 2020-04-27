using NetUtilities;
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// More extensions method for <see cref="Linq"/>.
    /// </summary>
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Batches the collection into a collection of collection of an specific size.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or selector are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a negative size is given.</exception>
        /// <typeparam name="TSource">The underlying type of the collection.</typeparam>
        /// <param name="source">The collection.</param>
        /// <param name="size">The size of the buckets.</param>
        /// <returns>An enumerable bulked by the given size.</returns>
        public static IEnumerable<IEnumerable<TSource>> BatchBy<TSource>(this IEnumerable<TSource> source, int size)
            => BatchBy(source, size, x => x);

        /// <summary>
        /// <summary>
        /// Batches the collection into a collection of collection of an specific size and returns the selected member.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or selector are null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a negative size is given</exception>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="size">The size of the buckets.</param>
        /// <param name="selector">The selector delegate.</param>
        /// <returns>An enumerable bulked by the given size.</returns>
        public static IEnumerable<TResult> BatchBy<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> selector)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (selector is null)
                Throw.NullArgument(nameof(selector));

            if (size < 0)
                Throw.InvalidOperation($"Batch size must be positive.");

            return BatchByIterator(source, selector, size);
        }

        private static IEnumerable<TResult> BatchByIterator<TSource, TResult>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, TResult> selector, int size)
        {
            using var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
                yield return selector(BatchByBulker(enumerator, size));
        }

        private static IEnumerable<TSource> BatchByBulker<TSource>(IEnumerator<TSource> enumerator, int size)
        {
            do 
                yield return enumerator.Current;
            while (--size > 0 && enumerator.MoveNext());
        }
    }
}

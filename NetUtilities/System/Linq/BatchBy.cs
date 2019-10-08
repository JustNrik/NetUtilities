using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq
{
    public static partial class LinqUtilities
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
        [return: NotNull]
        public static IEnumerable<IEnumerable<TSource>> BatchBy<TSource>(this IEnumerable<TSource> source, int size)
            => BatchBy(source, size, x => x);

        /// <summary>
        /// Bulks the collection into a collection of collection by an specific amount.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either source or selector are null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a negative size is given</exception>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="size">The size of the buckets.</param>
        /// <param name="selector">The selector delegate.</param>
        /// <returns>An enumerable bulked by the given size.</returns>
        [return: NotNull]
        public static IEnumerable<TResult> BatchBy<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> selector)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));
            if (selector is null)
                Throw.NullArgument(nameof(selector));
            if (size < 1)
                Throw.InvalidOperation($"Batch size must be higer than zero.");

            return BatchByIterator(source, selector, size);

            IEnumerable<TResult> BatchByIterator(IEnumerable<TSource> sequence, Func<IEnumerable<TSource>, TResult> picker, int count)
            {
                using var enumerator = source.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    var cts = new CancellationToken();
                    while (!cts.IsCancelled)
                        yield return picker(BatchByBulker(enumerator, count, cts));
                }

                static IEnumerable<TSource> BatchByBulker(IEnumerator<TSource> e, int count, CancellationToken token)
                {
                    for (var x = 0u; x < count; x++)
                    {
                        yield return e.Current;

                        if (!e.MoveNext())
                        {
                            token.Cancel();
                            yield break;
                        }

                    }
                }
            }
        }

        private class CancellationToken
        {
            public bool IsCancelled { get; private set; }
            public void Cancel()
                => IsCancelled = true;
        }
    }
}

namespace System.Linq
{
    using NetUtilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static partial class LinqUtilities
    {
        /// <summary>
        /// Bulks the collection into a collection of collection by an specific amount.
        /// </summary>
        /// <typeparam name="TSource">The underlying type of the collection</typeparam>
        /// <param name="source">The collection</param>
        /// <param name="size">The size of the buckets</param>
        /// <returns>An enumerable bulked by the given size.</returns>
        [return: NotNull]
        public static IEnumerable<IEnumerable<TSource>> BulkBy<TSource>(this IEnumerable<TSource> source, int size)
            => BulkBy(source, size, x => x);

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
        public static IEnumerable<TResult> BulkBy<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> selector)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(selector, nameof(selector));
            Ensure.Positive(size, nameof(size));

            return BulkByIterator();

            IEnumerable<TResult> BulkByIterator()
            {
                TSource[]? bucket = null;
                var count = 0;

                foreach (var item in source)
                {
                    if (bucket is null)
                        bucket = new TSource[size];


                    bucket[count++] = item;

                    if (count != size)
                        continue;
 

                    yield return selector(bucket);

                    bucket = null;
                    count = 0;
                }

                if (bucket is object && count > 0)
                {
                    Array.Resize(ref bucket, count);
                    yield return selector(bucket);
                }
            }
        }
    }
}

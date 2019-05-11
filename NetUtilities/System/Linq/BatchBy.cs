using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Bulks the collection into an <see cref="IEnumerable{IEnumerable{TSource}}"/> by an specific amount.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> BulkBy<TSource>(this IEnumerable<TSource> source, int size)
        {
            return BulkBy(source, size, x => x);
        }

        public static IEnumerable<TResult> BulkBy<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            return BulkByIterator();

            IEnumerable<TResult> BulkByIterator()
            {
                TSource[] bucket = null;
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

                // Return the last bucket with all remaining elements
                if (!(bucket is null) && count > 0)
                {
                    Array.Resize(ref bucket, count);
                    yield return selector(bucket);
                }
            }
        }
    }
}

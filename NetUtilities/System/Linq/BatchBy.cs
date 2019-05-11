using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Bulks the collection into an <see cref="IEnumerable{T}"/> by an specific amount.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T[]> BulkBy<T>(this IEnumerable<T> source, int count)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            var c = source.Count();
            var pivot = c / count;

            if (c % count != 0)
                pivot++;

            var length = 0;

            for (int x = 0; x < pivot; x++)
            {
                yield return InnerBulkBy(count).ToArray();
                length += count;
            }

            IEnumerable<T> InnerBulkBy(int count)
            {
                return source.Skip(length).Take(count);
            }
        }
    }
}

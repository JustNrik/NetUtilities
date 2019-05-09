using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Takes all elements until the predicate is True.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return TakeUntilIterator();

            IEnumerable<TSource> TakeUntilIterator()
            {
                foreach (var item in source)
                {
                    if (!predicate(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Takes all elements while the predicate is True.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return TakeWhileIterator();

            IEnumerable<TSource> TakeWhileIterator()
            {
                foreach (var item in source)
                {
                    if (!predicate(item)) break;

                    yield return item;
                }
            }
        }
    }
}

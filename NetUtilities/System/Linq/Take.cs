using NetUtilities;
using System.Collections.Generic;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Takes all elements until the predicate returns true.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return TakeUntilIterator();

            IEnumerable<TSource> TakeUntilIterator()
            {
                foreach (var item in source)
                {
                    if (predicate(item)) yield break;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Takes all elements while the predicate returns true.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            return TakeWhileIterator();

            IEnumerable<TSource> TakeWhileIterator()
            {
                foreach (var item in source)
                {
                    if (!predicate(item)) yield break;

                    yield return item;
                }
            }
        }

        /// <summary>
        /// Takes an amount of items from the source that matches the given predicate.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeIf<TSource>(this IEnumerable<TSource> source, int count, Predicate<TSource> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));
            Ensure.Positive(count, nameof(count));

            return TakeIfIterator();

            IEnumerable<TSource> TakeIfIterator()
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
}

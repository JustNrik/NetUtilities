using System.Collections.Generic;
using NetUtilities;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        ///     Uses the selector to return a collection without duplicates based on <see cref="EqualityComparer{TKey}.Default"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The underlying type of the source of the collection.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The type of the selected member.
        /// </typeparam>
        /// <param name="source">
        ///     The source collection.
        /// </param>
        /// <param name="selector">
        ///     The selector delegate used to filter duplicates.
        /// </param>
        /// <returns>
        ///     A sequence without duplicates based on <see cref="EqualityComparer{TKey}.Default"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either the source or the selector are <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(selector);

            return DistinctByIterator(source, selector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        ///     Uses the selector to return a collection without duplicates based on <see cref="EqualityComparer{TKey}.Default"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The underlying type of the source of the collection.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The type of the selected member.
        /// </typeparam>
        /// <param name="source">
        ///     The source collection.
        /// </param>
        /// <param name="selector">
        ///     The selector delegate used to filter duplicates.
        /// </param>
        /// <param name="comparer">The comparer used to filter duplicates.</param>
        /// <returns>
        ///     A sequence without duplicates based on <see cref="EqualityComparer{TKey}.Default"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either the source or the selector are <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(selector);

            return DistinctByIterator(source, selector, comparer);
        }

        private static IEnumerable<TSource> DistinctByIterator<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IEqualityComparer<TKey> comparer)
        {
            var keys = new HashSet<TKey>(comparer);
            foreach (var item in source)
            {
                if (keys.Add(selector(item)))
                    yield return item;
            }
        }
    }
}

using NetUtilities;
using System.ComponentModel;

namespace System.Collections.Generic
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GenericCollectionExtensions
    {
        /// <summary>
        /// Enumerates the provided collection with an index.
        /// </summary>
        /// <typeparam name="T">The underlying type of the collection.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>An collection enumerated by index.</returns>
        public static IEnumerable<(T Element, int Index)> AsIndexed<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source, nameof(source));

            return source is IReadOnlyList<T> list 
                ? IndexingListEnumerator(list)
                : IndexingEnumerator(source);

            static IEnumerable<(T, int)> IndexingEnumerator(IEnumerable<T> source)
            {
                var count = 0;
                foreach (var item in source)
                    yield return (item, count++);
            }

            static IEnumerable<(T, int)> IndexingListEnumerator(IReadOnlyList<T> source)
            {
                for (var index = 0; index < source.Count; index++)
                    yield return (source[index], index);
            }
        }

        /// <summary>
        /// Shuffles the provided collection. 
        /// You can optionally provide the amount of iterations. 
        /// High values may have a negative impact on performance.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="iterations">The amount of iterations to perform.</param>
        public static void Shuffle<T>(this IList<T> source, byte iterations = 4)
        {
            Ensure.NotNull(source, nameof(source));

            if (source.Count < 2)
                return;

            for (var count = 0; count < iterations; count++)
            {
                for (var index = 0; index < source.Count; index++)
                {
                    var next = Randomizer.Shared.Next(source.Count);
                    var current = source[index];

                    source[index] = source[next];
                    source[next] = current;
                }
            }
        }
    }
}

using NetUtilities;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for generic collections.
    /// </summary>
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
                    var nextIndex = Randomizer.Shared.Next(source.Count);
                    var next = source[nextIndex];
                    var current = source[index];

                    source[index] = next;
                    source[nextIndex] = current;
                }
            }
        }

        /// <summary>
        /// Adds an element to a <see cref="IList{T}"/> if it doesn't exists, otherwise it updates it.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ICollection{T}.IsReadOnly"/> returns true.</exception>
        /// <typeparam name="T">The type of the element to be added.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="element">The element to search.</param>
        public static void AddOrUpdate<T>(this IList<T> source, [AllowNull]T element)
        {
            Ensure.NotNull(source);

            if (source.IsReadOnly)
                Throw.InvalidOperation($"{source.GetType().Name} is a Read-Only collection");

            var index = source.IndexOf(element!);

            if (index == -1)
                source.Add(element!);
            else
                source[index] = element!;
        }

        /// <summary>
        /// Gets a value given the key, or adds it if it doesn't exists.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if either the source or the key are null.</exception>
        /// <exception cref="InvalidOperationException">Throw if <see cref="ICollection{T}.IsReadOnly"/> returns true.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The value.</returns>
        [return: MaybeNull]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            where TKey : notnull
        {
            Ensure.NotNull(source);
            Ensure.NotNull(key);

            if (source.IsReadOnly)
                Throw.InvalidOperation($"{source.GetType().Name} is a Read-Only collection");

            if (source.TryGetValue(key, out var val))
                return val;

            source.Add(key, value);
            return value;
        }
    }
}

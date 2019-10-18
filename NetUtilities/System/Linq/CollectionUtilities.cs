using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Adds an element to a <see cref="IList{T}"/> if it doesn't exists, otherwise it updates it.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="ICollection{T}.IsReadOnly"/> returns true.</exception>
        /// <typeparam name="T">The type of the element to be added.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="element">The element to search.</param>
        public static void AddOrUpdate<T>(this IList<T> source, T element)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source.IsReadOnly)
                Throw.InvalidOperation($"{source.GetType().Name} is a Read-Only collection"); 

            var index = source.IndexOf(element);

            if (index == -1)
                source.Add(element);
            else
                source[index] = element;
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
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (key is null)
                Throw.NullArgument(nameof(key));

            if (source.IsReadOnly)
                Throw.InvalidOperation($"{source.GetType().Name} is a Read-Only collection");

            if (source.TryGetValue(key, out var val))
                return val;

            source.Add(key, value);
            return value;
        }

        /// <summary>
        /// Shuffles the given list.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="source">The source list.</param>
        public static void Shuffle<T>(this IList<T> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source is T[] array)
            {
                Array.Sort(array, RandomComparer<T>.Instance);
                return;
            }

            if (source is List<T> list)
            {
                list.Sort(RandomComparer<T>.Instance);
                return;
            }

            var maxIter = (int)Math.Log(source.Count);

            for (var _ = 0; _ < maxIter; _++)
            {
                for (var index = 0; index < source.Count - 1; index++)
                {
                    var current = source[index];
                    var next = source[index + 1];

                    if (RandomComparer<T>.Instance.Compare(current, next) != 0)
                    {
                        source[index] = next;
                        source[index + 1] = current;
                    }
                }
            }
        }
    }
}
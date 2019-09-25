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
            Ensure.NotNull(source, nameof(source));
            Ensure.CanWrite(source);

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
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(key, nameof(key));
            Ensure.CanWrite(source);

            if (source.TryGetValue(key, out var val))
                return val;

            source.Add(key, value);
            return value;
        }
    }
}

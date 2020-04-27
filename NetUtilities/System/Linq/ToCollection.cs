using NetUtilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// Turns an sequence of <see cref="KeyValuePair{TKey, TValue}"/> 
        /// into a <see cref="Dictionary{TKey, TValue}"/> using the key and value of the pair.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with the given sequence.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
            where TKey : notnull
            => Ensure.NotNull(source, nameof(source)).ToDictionary(x => x.Key, x => x.Value);


        /// <summary>
        /// Turns an sequence of <see cref="KeyValuePair{TKey, TValue}"/> 
        /// into a <see cref="Dictionary{TKey, TValue}"/> using the key and value of the pair with the comparer provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source or comparer are null.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with the given sequence.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source, 
            IEqualityComparer<TKey> comparer)
            where TKey : notnull
            => Ensure.NotNull(source, nameof(source)).ToDictionary(x => x.Key, x => x.Value, Ensure.NotNull(comparer, nameof(comparer)));


        /// <summary>
        /// Turns an sequence of <see cref="ValueTuple{TKey, TValue}"/> 
        /// into a <see cref="Dictionary{TKey, TValue}"/> using the key and value of the pair.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with the given sequence.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source)
           where TKey : notnull
            => Ensure.NotNull(source, nameof(source)).ToDictionary(x => x.Key, x => x.Value);


        /// <summary>
        /// Turns an sequence of <see cref="ValueTuple{TKey, TValue}"/> 
        /// into a <see cref="Dictionary{TKey, TValue}"/> using the key and value of the pair with the comparer provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with the given sequence.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<(TKey key, TValue value)> source, 
            IEqualityComparer<TKey> comparer)
            where TKey : notnull
            => Ensure.NotNull(source, nameof(source)).ToDictionary(x => x.key, x => x.value, Ensure.NotNull(comparer, nameof(comparer)));

        /// <summary>
        /// Creates a <see cref="ReadOnlyDictionary{TKey, TValue}"/> from the dictionary provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <returns>A Read-Only Dictionary from the dictionary provided.</returns>
        public static IReadOnlyDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> source)
            where TKey : notnull
            => new ReadOnlyDictionary<TKey, TValue>(Ensure.NotNull(source, nameof(source)));

        /// <summary>
        /// Creates as <see cref="ReadOnlyList{T}"/> from the sequence provided.
        /// </summary>
        /// <typeparam name="T">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <returns>A Read-Only list.</returns>
        public static ReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
            => new ReadOnlyList<T>(Ensure.NotNull(source, nameof(source)));
    }
}

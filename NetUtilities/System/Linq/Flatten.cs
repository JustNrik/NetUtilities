namespace System.Linq
{
    using NetUtilities;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static partial class LinqUtilities
    {
        /// <summary>
        /// Flattens the given sequence of sequences into a single sequence.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source is null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <returns>A sequence with all members of the original sequence and the nested sequence.</returns>
        [return: NotNull]
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        {
            Ensure.NotNull(source, nameof(source));
            return source.SelectMany(x => x);
        }

        /// <summary>
        /// Flattens the given sequence of sequences into a single sequence given a selector.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either the source or selector are null.</exception>
        /// <typeparam name="TSource">The underlying type of the sequence of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="selector">The selector used to select the nested sequence.</param>
        /// <returns>A sequence with all members of the original sequence and the nested sequence.</returns>
        [return: NotNull]
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source, Func<IEnumerable<TSource>, IEnumerable<TSource>> selector)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(selector, nameof(selector));
            return source.SelectMany(selector);
        }
    }
}

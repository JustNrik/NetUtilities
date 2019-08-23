namespace System.Linq
{
    using NetUtilities;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static partial class LinqUtilities
    {
        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throw when the source is null.</exception>
        /// <param name="source">The string sequence.</param>
        /// <param name="separator">The separator used to join the strings. <see cref="string.Empty"/> if no separator is provided.</param>
        /// <returns>A single <see cref="string"/> with all the elements of the sequence.</returns>
        [return: NotNull]
        public static string Join(this IEnumerable<string> source, string? separator = null)
            => string.Join(separator ?? string.Empty, Ensure.NotNull(source, nameof(source)));


        /// <summary>
        /// Joins the sequence of chars into a single string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throw when the source is null.</exception>
        /// <param name="source">The char sequence.</param>
        /// <param name="separator">The separator used to join the chars. <see cref="string.Empty"/> if no separator is provided.</param>
        /// <returns>A single <see cref="string"/> with all the elements of the sequence.</returns>
        [return: NotNull]
        public static string Join(this IEnumerable<char> source, string? separator = null)
            => string.Join(separator ?? string.Empty, Ensure.NotNull(source, nameof(source)));
    }
}

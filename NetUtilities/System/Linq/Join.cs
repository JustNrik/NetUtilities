using NetUtilities;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        ///     Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="source">
        ///     The string sequence.
        /// </param>
        /// <param name="separator">
        ///     The separator used to join the strings. <see cref="string.Empty"/> if no separator is provided.
        /// </param>
        /// <returns>
        ///     A single <see cref="string"/> with all the elements of the sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when the source is <see langword="null"/>.
        /// </exception>
        public static string Join(this IEnumerable<string> source, string? separator = null)
            => string.Join(separator ?? string.Empty, Ensure.NotNull(source));


        /// <summary>
        ///     Joins the sequence of chars into a single string.
        /// </summary>
        /// <param name="source">
        ///     The char sequence.
        /// </param>
        /// <param name="separator">
        ///     The separator used to join the chars. <see cref="string.Empty"/> if no separator is provided.
        /// </param>
        /// <returns>
        ///     A single <see cref="string"/> with all the elements of the sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when the source is <see langword="null"/>.
        /// </exception>
        public static string Join(this IEnumerable<char> source, string? separator = null)
            => string.Join(separator ?? string.Empty, Ensure.NotNull(source));
    }
}

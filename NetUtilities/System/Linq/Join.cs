using NetUtilities;
using System.Collections.Generic;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Joins the sequence of strings into a single string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            Ensure.NotNull(source, nameof(source));
            return string.Join(separator ?? string.Empty, source);
        }

        /// <summary>
        /// Joins the sequence of chars into a single string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<char> source, string separator)
        {
            Ensure.NotNull(source, nameof(source));
            return string.Join(separator ?? string.Empty, source);
        }
    }
}

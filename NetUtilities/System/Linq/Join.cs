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
            if (source is null) throw new ArgumentNullException(nameof(source));
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
            if (source is null) throw new ArgumentNullException(nameof(source));

            return string.Join(separator ?? string.Empty, source);
        }
    }
}

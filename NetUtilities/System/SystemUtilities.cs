#nullable enable
namespace System
{
    public static class SystemUtilities
    {
        /// <summary>
        /// Creates a <see cref="string"/> with the providen array
        /// </summary>
        /// <param name="charArray"></param>
        /// <returns></returns>
        public static string AsString(this char[]? charArray)
            => new string(charArray);

        /// <summary>
        /// Gets the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        public static string SubstringBetween(this string? input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start == 0) return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end == -1 || start > end) return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[(start..end)!];
        }

        /// <summary>
        /// Gets the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        public static string SubstringBetween(this MutableString? input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start == 0) return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end == -1 || start > end) return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[start..end];
        }

        public static MutableString ToMutable(this string? value)
            => new MutableString(value);
    }
}

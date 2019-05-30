#nullable enable
using System.Collections.Generic;

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

        /// <summary>
        /// Gets an array of substrings between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        public static string[] SubstringsBetween(this MutableString? input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var indexes = input.IndexesOf(leftBound, startIndex);

            if (indexes.Length == 0) return Array.Empty<string>();

            var result = new string[indexes.Length];

            foreach (var index in indexes)
                result[index] = input.SubstringBetween(leftBound, rightBound, index, includeBounds);

            return result;
        }

        /// <summary>
        /// Gets an array of substrings between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="includeBounds"></param>
        /// <returns></returns>
        public static string[] SubstringsBetween(this string? input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var indexes = input.IndexesOf(leftBound, startIndex, input.Length);

            if (indexes.Length == 0) return Array.Empty<string>();

            var result = new string[indexes.Length];

            foreach (var index in indexes)
                result[index] = input.SubstringBetween(leftBound, rightBound, index, includeBounds);

            return result;
        }

        /// <summary>
        /// Gets all the indexes of the providen <see cref="char"/>, this method returns an empty array if no indexes are found.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] IndexesOf(this string? input, char value, int startIndex, int count)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            if (count == 0) return Array.Empty<int>();

            var currentIndex = input.IndexOf(value, startIndex, count);
            var result = new List<int>(5);

            while (currentIndex != -1)
            {
                result.Add(currentIndex);
                currentIndex = input.IndexOf(value, currentIndex + 1);
            }

            return result.ToArray();
        }

        public static MutableString ToMutable(this string? value)
            => new MutableString(value);
    }
}
using System.Runtime.CompilerServices;
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
        public static string SubstringBetween(this string? input, char leftBound, char rightBound, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var start = input.IndexOf(leftBound) + 1;

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
        public static string SubstringBetween(this MutableString? input, char leftBound, char rightBound, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var start = input.IndexOf(leftBound) + 1;

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

        // I tested this against Math.Log method, this eye-cancer thing is by far the fastest.
        // Inlining it makes it 3 times faster. (enjoy these seven nanoseconds)
        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this uint value)
            => GetDigits((ulong)value);

        /// <summary>
        /// Gets the amount of difits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this int value)
            => value == int.MinValue ? 10 : GetDigits((ulong)Math.Abs(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this long value)
            => value == long.MinValue ? 19 : GetDigits((ulong)Math.Abs(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this ulong value)
        {
            if (value < 10UL) return 1;
            if (value < 100UL) return 2;
            if (value < 1000UL) return 3;
            if (value < 10000UL) return 4;
            if (value < 100000UL) return 5;
            if (value < 1000000UL) return 6;
            if (value < 10000000UL) return 7;
            if (value < 100000000UL) return 8;
            if (value < 1000000000UL) return 9;
            if (value < 10000000000UL) return 10;
            if (value < 100000000000UL) return 11;
            if (value < 1000000000000UL) return 12;
            if (value < 10000000000000UL) return 13;
            if (value < 100000000000000UL) return 14;
            if (value < 1000000000000000UL) return 15;
            if (value < 10000000000000000UL) return 16;
            if (value < 100000000000000000UL) return 17;
            if (value < 1000000000000000000UL) return 18;
            if (value < 10000000000000000000UL) return 19;
            return 20;
        }

        public static MutableString ToMutable(this string? value)
            => new MutableString(value);
    }
}

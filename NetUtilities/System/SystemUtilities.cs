using System.Runtime.CompilerServices;
#nullable enable
namespace System
{
    public static class SystemUtilities
    {
        public static string AsString(this char[] charArray)
            => new string(charArray);

        public static string SubstringBetween(this string input, char leftBound, char rightBound, bool includeBounds = false)
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

        public static MutableString SubstringBetween(this MutableString input, char leftBound, char rightBound, bool includeBounds = false)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var start = input.IndexOf(leftBound) + 1;

            if (start == 0) return new MutableString();

            var end = input.IndexOf(rightBound, start);

            if (end == -1 || start > end) return new MutableString();

            if (includeBounds)
            {
                start--;
                end++;
            }

            var result = new MutableString(end - start + 1);

            for (int index = start; start <= end; index++)
            {
                result += input[index];
            }

            return result;
        }

        // I tested this against Math.Log method, this eye-cancer thing is by far the fastest.
        // Inlining it makes it 3 times faster. (enjoy these seven nanoseconds)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this int value)
        {
            value = Math.Abs(value);
            if (value < 10) return 1;
            if (value < 100) return 2;
            if (value < 1000) return 3;
            if (value < 10000) return 4;
            if (value < 100000) return 5;
            if (value < 1000000) return 6;
            if (value < 10000000) return 7;
            if (value < 100000000) return 8;
            if (value < 1000000000) return 9;
            return 10;
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NetUtilities;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SystemUtilities
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        ///     Creates a <see cref="string"/> with the provided array.
        /// </summary>
        /// <param name="charArray">
        ///     The array.
        /// </param>
        /// <returns>
        ///     A new <see cref="string"/> built from the array provided.
        /// </returns>
        public static string AsString(this char[]? charArray)
            => new string(charArray);

        /// <summary>
        ///     Creates a <see cref="string"/> with the provided span.
        /// </summary>
        /// <param name="charSpan">
        ///     The span.
        /// </param>
        /// <returns>
        ///     A new <see cref="string"/> build from the span provided.
        /// </returns>
        public static string AsString(this ReadOnlySpan<char> charSpan)
            => new string(charSpan);

        /// <summary>
        ///     Returns the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input">
        ///     The input.
        /// </param>
        /// <param name="leftBound">
        ///     The left bound.
        /// </param>
        /// <param name="rightBound">
        ///     The right bound.
        /// </param>
        /// <param name="includeBounds">
        ///     Indicates if bounds should be included in the substring.
        /// </param>
        /// <returns>
        ///     The substring between two <see cref="char"/> delimiters.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when the input is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the startIndex is greater than or equal to input's lenght.
        /// </exception>
        public static string SubstringBetween(this string input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            Ensure.NotNull(input);
            Ensure.IsInRange(startIndex < input.Length, startIndex);

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start is 0)
                return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end is -1 || start > end)
                return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[start..end];
        }

        /// <summary>
        ///     Returns the substring between two <see cref="char"/> delimiters. Optionally, you can include them into the result.
        /// </summary>
        /// <param name="input">
        ///     The input.
        /// </param>
        /// <param name="leftBound">
        ///     The left bound.
        /// </param>
        /// <param name="rightBound">
        ///     The right bound.
        /// </param>
        /// <param name="includeBounds">
        ///     Indicates if bounds should be included in the substring.
        /// </param>
        /// <returns>
        ///     The substring between two <see cref="char"/> delimiters.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when the input is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the startIndex is greater than or equal to input's lenght.
        /// </exception>
        public static string SubstringBetween(this MutableString input, char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false)
        {
            Ensure.NotNull(input); 
            Ensure.IsInRange(startIndex < input.Length, startIndex);

            var start = input.IndexOf(leftBound, startIndex) + 1;

            if (start is 0)
                return string.Empty;

            var end = input.IndexOf(rightBound, start);

            if (end is -1 || start > end)
                return string.Empty;

            if (includeBounds)
            {
                start--;
                end++;
            }

            return input[start..end];
        }

        /// <summary>
        ///     Gets all the indexes of the provided <see cref="char"/>, this method returns an empty array if no indexes are found.
        /// </summary>
        /// <param name="input">
        ///     The input.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="startIndex">
        ///     The starting index.
        /// </param>
        /// <param name="count">
        ///     The amount of characters to search
        /// </param>
        /// <returns>
        ///     A <see cref="ReadOnlyList{T}"/> with all indexes found.
        /// </returns>
        public static ReadOnlyList<int> IndexesOf(this string input, char value, int startIndex, int count)
        {
            Ensure.NotNull(input);

            if (count is 0)
                return ReadOnlyList<int>.Empty;

            var currentIndex = input.IndexOf(value, startIndex, count);

            if (currentIndex is -1)
                return ReadOnlyList<int>.Empty;

            var result = new List<int>();

            do
                result.Add(currentIndex);
            while ((currentIndex = input.IndexOf(value, currentIndex + 1)) is not -1);

            return new ReadOnlyList<int>(result, true);
        }

        /// <summary>
        ///     Creates a <see cref="MutableString"/> with the <see cref="string"/> provided.
        /// </summary>
        /// <param name="value">
        ///     The string.
        /// </param>
        /// <returns>
        ///     A <see cref="MutableString"/>.
        /// </returns>
        public static MutableString ToMutable(this string value)
            => new MutableString(value);

        /// <summary>
        ///     Returns a new <see cref="string"/> with the reversed characters.
        /// </summary>
        /// <param name="str">
        ///     The string to be reversed.
        /// </param>
        /// <returns>
        ///     A reversed string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the provided string is <see langword="null"/>.
        /// </exception>
        public static string Reverse(this string str)
            => string.Create(Ensure.NotNull(str).Length, str,
                (span, state) =>
                {
                    for (int i = 0, j = span.Length - 1; i < span.Length; i++, j--)
                        span[i] = state[j];
                });

        /// <summary>
        ///     Returns a boolean indicating if both strings are similar content-wise (case-insensitive).
        /// </summary>
        /// <param name="str">
        ///     The string to be compared.
        /// </param>
        /// <param name="other">
        ///     The string to look for similarities.
        /// </param>
        /// <param name="comparison">
        ///     The comparison to be used to determine if they are alike.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the strings are similar (case-insensitive); otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either str or other are <see langword="null"/>.
        /// </exception>
        public static bool Like(this string str, string other)
            => Ensure.NotNull(str).Equals(Ensure.NotNull(other), StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Indicates if the string contains any of the words provided.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <param name="words">
        ///     The words.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the string contains any of the provided words, otherwise, <see langword="false"=""/>.
        /// </returns>
        public static bool ContainsAny(this string str, params string[] words)
        {
            Ensure.NotNull(str);
            Ensure.NotNull(words);

            foreach (var word in words)
            {
                if (str.Contains(word))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Indicates if the string contains any of the words provided.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison to be used.
        /// </param>
        /// <param name="words">
        ///     The words.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the string contains any of the provided words, otherwise, <see langword="false"=""/>.
        /// </returns>
        public static bool ContainsAny(this string str, StringComparison comparisonType, params string[] words)
        {
            Ensure.NotNull(str);
            Ensure.NotNull(words);

            foreach (var word in words)
            {
                if (str.Contains(word, comparisonType))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Indicates if the string contains all of the words provided.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <param name="words">
        ///     The words.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the string contains all of the provided words, otherwise, <see langword="false"/>.
        /// </returns>
        public static bool ContainsAll(this string str, params string[] words)
        {
            Ensure.NotNull(str);
            Ensure.NotNull(words);

            foreach (var word in words)
            {
                if (!str.Contains(word))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     Indicates if the string contains all of the words provided.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison to be used.
        /// </param>
        /// <param name="words">
        ///     The words.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the string contains all of the provided words, otherwise, <see langword="false"/>.
        /// </returns>
        public static bool ContainsAll(this string str, StringComparison comparisonType, params string[] words)
        {
            Ensure.NotNull(str);
            Ensure.NotNull(words);

            foreach (var word in words)
            {
                if (!str.Contains(word, comparisonType))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     Quotes the given <see cref="string"/>.
        /// </summary>
        /// <param name="str">
        ///     The string.
        /// </param>
        /// <returns>A quoted <see cref="string"/>.</returns>
        public static string Quote(this string str)
            => string.Create(Ensure.NotNull(str).Length + 2, str, (span, state) =>
            {
                span[0] = '"';
                span[^1] = '"';

                for (var index = 1; index < span.Length - 1; index++)
                    span[index] = state[index - 1];
            });

        /// <summary>
        ///     Checks if the runtime type of obj is the targeted one.
        /// </summary>
        /// <typeparam name="T">
        ///     Target type.
        /// </typeparam>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the runtime type of obj is the targeted one; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool IsType<T>(object? obj)
            => IsType(obj, typeof(T));

        /// <summary>
        ///     Checks if the runtime type of obj is the targeted one.
        /// </summary>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <param name="targetType">
        ///     The type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the runtime type of obj is the targeted one; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool IsType(object? obj, Type targetType)
            => obj?.GetType() == targetType;

        /// <summary>
        ///     Gets each flags of the given <typeparamref name="TEnum"/> instance.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     Type of the <see cref="Enum"/>.
        /// </typeparam>
        /// <param name="enum">
        ///     The <see cref="Enum"/> object.
        /// </param>
        /// <returns>
        ///     A <typeparamref name="TEnum"/>[] containing all the flags found in the current instance.
        /// </returns>
        public static TEnum[] GetFlags<TEnum>(this TEnum @enum) where TEnum : unmanaged, Enum
        {
            EnumHelper<TEnum>.ThrowIfNotFlagEnum();

            var values = EnumHelper<TEnum>.Values;
            var length = 0;
            Span<TEnum> span = stackalloc TEnum[values.Length];

            foreach (var value in values)
            {
                if (@enum.HasFlag(value))
                    span[length++] = value;
            }

            return span[..length].ToArray();
        }

        private static class EnumHelper<TEnum> where TEnum : unmanaged, Enum
        {
            private static readonly bool _isFlag = typeof(TEnum).GetCustomAttribute<FlagsAttribute>() is not null;

            public static TEnum[] Values { get; } = Enum.GetValues<TEnum>()
                .Where(x =>
                {
                    var value = Unsafe.As<TEnum, long>(ref x);
                    return (value & (value - 1)) == 0 && value != 0;
                }).ToArray();

            public static void ThrowIfNotFlagEnum()
            {
                if (!_isFlag)
                    throw new InvalidOperationException("This method can only be used on enums with FlagsAttributes");
            }
        }
    }
}
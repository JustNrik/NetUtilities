using NetUtilities;
using System.Diagnostics.CodeAnalysis;

namespace System.Text
{
    public static class StringBuilderUtils
    {
        [return: NotNull]
        public static StringBuilder AppendIf([NotNull]this StringBuilder builder, bool condition, string text)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(text) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendIf([NotNull]this StringBuilder builder, bool condition, char c)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(c) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendIf([NotNull]this StringBuilder builder, bool condition, string text, int startIndex, int count)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(text, startIndex, count) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendLineIf([NotNull]this StringBuilder builder, bool condition, string text)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.AppendLine(text) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendFormatIf(
            [NotNull]this StringBuilder builder, bool condition,
            [NotNull]string format, 
            [NotNull]params object[] args)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition 
                ? builder.AppendFormat(
                    Ensure.NotNull(format, nameof(format)), 
                    Ensure.NotNull(args, nameof(args))) 
                : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendFormatIf(
            [NotNull]this StringBuilder builder, bool condition,
            [NotNull]IFormatProvider provider,
            [NotNull]string format,
            [NotNull]params object[] args)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition 
                ? builder.AppendFormat(
                    Ensure.NotNull(provider, nameof(provider)), 
                    Ensure.NotNull(format, nameof(format)), 
                    Ensure.NotNull(args, nameof(args))) 
                : builder;
        }
    }
}

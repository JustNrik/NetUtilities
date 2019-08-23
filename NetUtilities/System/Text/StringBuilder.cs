namespace System.Text
{
    using NetUtilities;
    using System.Diagnostics.CodeAnalysis;

    public static class StringBuilderUtils
    {
        [return: NotNull]
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string text)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(text) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, char c)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(c) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string text, int startIndex, int count)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.Append(text, startIndex, count) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendLineIf(this StringBuilder builder, bool condition, string text)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.AppendLine(text) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, params object[] args)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.AppendFormat(format, args) : builder;
        }

        [return: NotNull]
        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, IFormatProvider provider, string format, params object[] args)
        {
            Ensure.NotNull(builder, nameof(builder));
            return condition ? builder.AppendFormat(provider, format, args) : builder;
        }
    }
}

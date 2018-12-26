namespace System.Text
{
    public static class StringBuilderUtils
    {
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string text)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            return condition ? builder.Append(text) : builder;
        }

        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string text, int startIndex, int count)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            return condition ? builder.Append(text, startIndex, count) : builder;
        }

        public static StringBuilder AppendLineIf(this StringBuilder builder, bool condition, string text)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            return condition ? builder.AppendLine(text) : builder;
        }

        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, string format, params object[] args)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            return condition ? builder.AppendFormat(format, args) : builder;
        }

        public static StringBuilder AppendFormatIf(this StringBuilder builder, bool condition, IFormatProvider provider, string format, params object[] args)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            return condition ? builder.AppendFormat(provider, format, args) : builder;
        }
    }
}

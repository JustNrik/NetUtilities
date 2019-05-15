using System.Collections.Generic;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.SelectMany(x => x);
        }

        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source, Func<IEnumerable<TSource>, IEnumerable<TSource>> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return source.SelectMany(selector);
        }
    }
}

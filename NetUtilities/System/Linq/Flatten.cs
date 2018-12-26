using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.SelectMany(x => x);
        }
    }
}

using NetUtilities;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TTarget> Select<TSource, TTarget>(this IAsyncEnumerable<TSource> source, Func<TSource, TTarget> selector)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (selector is null)
                Throw.NullArgument(nameof(selector));

            return SelectIterator(source, selector);
        }

        private static async IAsyncEnumerable<TTarget> SelectIterator<TSource, TTarget>(IAsyncEnumerable<TSource> sequence, Func<TSource, TTarget> func)
        {
            await foreach (var item in sequence)
                yield return func(item);
        }
    }
}

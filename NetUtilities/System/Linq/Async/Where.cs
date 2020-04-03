using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            return WhereIterator(source, predicate);
        }

        private static async IAsyncEnumerable<TSource> WhereIterator<TSource>(IAsyncEnumerable<TSource> sequence, Func<TSource, bool> func)
        {
            await new SynchronizationContextRemover();

            await foreach (var item in sequence)
            {
                if (func(item))
                    yield return item;
            }
        }
    }
}

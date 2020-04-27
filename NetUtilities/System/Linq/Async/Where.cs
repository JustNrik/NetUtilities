using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

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

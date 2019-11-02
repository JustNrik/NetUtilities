using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            await new SynchronizationContextRemover();

            var (any, item) = await TryGetFirstAsync(source);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            return item;
        }

        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await new SynchronizationContextRemover();

            var (any, item) = await TryGetFirstAsync(source, predicate).ConfigureAwait(false);

            if (!any)
                Throw.InvalidOperation("sequence contains no matching elements");

            return item;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            await new SynchronizationContextRemover();

            return (await TryGetFirstAsync(source)).Item;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await new SynchronizationContextRemover();

            return (await TryGetFirstAsync(source, predicate)).Item;
        }

        private async static ValueTask<(bool Any, TSource Item)> TryGetFirstAsync<TSource>(IAsyncEnumerable<TSource> source)
        {
            await foreach (var item in source)
                return (true, item);

            return (false, default!);
        }

        private static async ValueTask<(bool Any, TSource Item)> TryGetFirstAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            await foreach (var item in source)
            {
                if (predicate(item))
                    return (true, item);
            }

            return (false, default!);
        }
    }
}
using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static async ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            var (any, item) = await TryGetLastAsync(source).ConfigureAwait(false);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            return item;
        }

        public static async ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            var (any, item) = await TryGetLastAsync(source, predicate).ConfigureAwait(false);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            return item;
        }

        public static async ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            return (await TryGetLastAsync(source).ConfigureAwait(false)).Item;
        }

        public static async ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            return (await TryGetLastAsync(source, predicate).ConfigureAwait(false)).Item;
        }

        private static ValueTask<(bool Any, TSource Item)> TryGetLastAsync<TSource>(IAsyncEnumerable<TSource> source)
            => TryGetLastAsync(source, _ => true);

        private static async ValueTask<(bool Any, TSource Item)> TryGetLastAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool found = false;
            TSource last = default!;

            await foreach (var item in source)
            {
                if (predicate(item))
                {
                    found = true;
                    last = item;
                }
            }

            return (found, last);
        }

        private static (bool, TSource) TryGetLast<TSource>(IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
        {
            for (var index = source.Count - 1; index >= 0; index--)
            {
                var item = source[index];

                if (predicate(item))
                    return (true, item);
            }

            return (false, default!);
        }
    }
}
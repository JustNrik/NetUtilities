using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        /// <inheritdoc cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.FirstAsync(True);

        /// <inheritdoc cref="Enumerable.First{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            var (any, item) = await TryGetFirstAsync(source, predicate).ConfigureAwait(false);

            if (!any)
                Throw.InvalidOperation("sequence contains no matching elements");

            return item;
        }

        /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.FirstOrDefaultAsync(True);

        /// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            return (await TryGetFirstAsync(source, predicate)).Item;
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
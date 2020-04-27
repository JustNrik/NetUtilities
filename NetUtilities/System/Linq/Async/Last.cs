using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        /// <inheritdoc cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.LastAsync(True);

        /// <inheritdoc cref="Enumerable.Last{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            var (any, item) = await TryGetLastAsync(source, predicate);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            return item;
        }

        /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.LastOrDefaultAsync(True);

        /// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();
            return (await TryGetLastAsync(source, predicate)).Item;
        }

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
    }
}
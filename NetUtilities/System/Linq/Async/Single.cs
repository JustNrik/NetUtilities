using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        /// <inheritdoc cref="Enumerable.Single{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.SingleAsync(True);

        /// <inheritdoc cref="Enumerable.Single{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            var (any, many, item) = await TryGetSingleAsync(source, predicate);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            if (many)
                Throw.InvalidOperation("sequence contains more than a single element");

            return item;
        }

        /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => source.SingleOrDefaultAsync(True);

        /// <inheritdoc cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            var (_, many, item) = await TryGetSingleAsync(source, predicate);

            if (many)
                Throw.InvalidOperation("sequence contains more than a single matching element");

            return item;
        }

        private static async ValueTask<(bool Any, bool Many, TSource Item)> TryGetSingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool any = false;
            bool many = false;
            TSource item = default!;

            await foreach (var element in source)
            {
                if (predicate(element))
                {
                    if (any)
                    {
                        many = true;
                        item = default!;
                        break;
                    }

                    any = true;
                    item = element;
                }
            }

            return (any, many, item);
        }
    }
}

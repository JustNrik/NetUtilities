using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => SingleAsync(source, _ => true);

        public static async ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await new SynchronizationContextRemover();

            var (any, many, item) = await TryGetSingleAsync(source, predicate);

            if (!any)
                Throw.InvalidOperation("sequence contains no elements");

            if (many)
                Throw.InvalidOperation("sequence contains more than a single element");

            return item;
        }

        public static ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => SingleOrDefaultAsync(source, _ => true);

        public static async ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

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

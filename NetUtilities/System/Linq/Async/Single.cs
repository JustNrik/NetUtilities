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

            var (_, many, item) = await TryGetSingleAsync(source, predicate);

            if (many)
                Throw.InvalidOperation("sequence contains more than a single matching element");

            return item;
        }

        private static async ValueTask<(bool any, bool many, TSource item)> TryGetSingleAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            await using var enumerator = source.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                var single = enumerator.Current;

                if (await enumerator.MoveNextAsync())
                    return (true, true, default!);

                return (true, false, single);
            }

            return (false, false, default!);
        }

        private static async ValueTask<(bool any, bool many, TSource item)> TryGetSingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool found = false;
            TSource single = default!;

            await foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (found)
                        return (true, true, default!);

                    found = true;
                    single = item;
                }
            }

            return (found, false, single);
        }
    }
}

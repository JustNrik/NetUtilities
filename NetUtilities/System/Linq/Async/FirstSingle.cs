using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class AsyncLinqExtensions
    {
        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            Ensure.NotNull(source, nameof(source));

            await foreach (var item in source)
                return item;

            throw new InvalidOperationException("sequence contains no elements");
        }

        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            await foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException("sequence contains no matching elements");
        }

        public static async ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            Ensure.NotNull(source, nameof(source));

            TSource ret;

            await using var enumerator = source.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                ret = enumerator.Current;
                if (await enumerator.MoveNextAsync())
                    throw new InvalidOperationException("sequence contains more than 1 element");
                else
                    return ret;
            }
                
            throw new InvalidOperationException("sequence contains no elements");
        }

        public static async ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            TSource ret = default!;
            bool found = false;

            await using var enumerator = source.GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                if (predicate(enumerator.Current))
                {
                    if (found)
                        throw new InvalidOperationException("sequence contains more than 1 matching element");

                    ret = enumerator.Current;
                    found = true;
                }
            }

            if (!found)
                throw new InvalidOperationException("sequence contains no elements");

            return ret;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            Ensure.NotNull(source, nameof(source));

            await foreach (var item in source)
                return item;

            return default!;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            await foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            return default!;
        }

        public static async ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            Ensure.NotNull(source, nameof(source));

            TSource ret;

            await using var enumerator = source.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
            {
                ret = enumerator.Current;
                if (await enumerator.MoveNextAsync())
                    throw new InvalidOperationException("sequence contains more than 1 element");
                else
                    return ret;
            }

            return default!;
        }

        public static async ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(predicate, nameof(predicate));

            TSource ret = default!;
            bool found = false;

            await using var enumerator = source.GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                if (predicate(enumerator.Current))
                {
                    if (found)
                        throw new InvalidOperationException("sequence contains more than 1 matching element");

                    ret = enumerator.Current;
                    found = true;
                }
            }

            return ret;
        }
    }
}

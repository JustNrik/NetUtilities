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

            var (success, value) = await TryGetFirstAsync(source);

            if (!success)
                Throw.InvalidOperation("sequence contains no elements");

            return value;
        }

        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            var (success, value) = await TryGetFirstAsync(source, predicate);

            if (!success)
                Throw.InvalidOperation("sequence contains no matching elements");

            return value;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            var (_, value) = await TryGetFirstAsync(source);
            return value;
        }

        public static async ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            var (_, value) = await TryGetFirstAsync(source, predicate);
            return value;
        }

        private static ValueTask<(bool, TSource)> TryGetFirstAsync<TSource>(IAsyncEnumerable<TSource> source)
            => TryGetFirstAsync(source, _ => true);

        private static async ValueTask<(bool, TSource)> TryGetFirstAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
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
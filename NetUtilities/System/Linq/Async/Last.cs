using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source is IReadOnlyList<TSource> list)
            {
                if (list.Count > 0)
                    return new ValueTask<TSource>(list[^1]);

                Throw.InvalidOperation("sequence contains no elements");
            }

            return LastAsyncFallback(source);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence)
            {
                var (found, last) = await TryGetLastAsync(sequence);

                if (!found)
                    Throw.InvalidOperation("sequence contains no elements");

                return last;
            }
        }

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            if (source is IReadOnlyList<TSource> list)
            {
                if (list.Count > 0)
                {
                    var (found, last) = TryGetLast(list, predicate);

                    if (!found)
                        Throw.InvalidOperation("sequence contains no elements");

                    return new ValueTask<TSource>(last);
                }

                Throw.InvalidOperation("sequence contains no elements");
            }

            return LastAsyncFallback(source, predicate);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence, Func<TSource, bool> func)
            {
                var (found, last) = await TryGetLastAsync(sequence);

                if (!found)
                    Throw.InvalidOperation("sequence contains no elements");

                return last;
            }
        }
        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source is IReadOnlyList<TSource> list)
            {
                if (list.Count > 0)
                    return new ValueTask<TSource>(list[^1]);

                return default;
            }

            return LastAsyncFallback(source);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence)
            {
                var (_, last) = await TryGetLastAsync(sequence);
                return last;
            }
        }
        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            if (source is IReadOnlyList<TSource> list)
            {
                var (_, last) = TryGetLast(list, predicate);
                return new ValueTask<TSource>(last);
            }

            return LastAsyncFallback(source, predicate);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence, Func<TSource, bool> func)
            {
                var (_, last) = await TryGetLastAsync(sequence, func);
                return last;
            }
        }

        private static ValueTask<(bool, TSource)> TryGetLastAsync<TSource>(IAsyncEnumerable<TSource> source)
            => TryGetLastAsync(source, _ => true);

        private static async ValueTask<(bool, TSource)> TryGetLastAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
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
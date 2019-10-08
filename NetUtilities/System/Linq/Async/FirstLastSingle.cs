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

            await foreach (var item in source)
                return item;

            Throw.InvalidOperation("sequence contains no elements");
            return default;
        }

        public static async ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }

            Throw.InvalidOperation("sequence contains no matching elements");
            return default;
        }

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source is IReadOnlyList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return new ValueTask<TSource>(list[list.Count - 1]);
                }

                Throw.InvalidOperation("sequence contains no elements");
            }

            return LastAsyncFallback(source);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence)
            {
                await using var enumerator = sequence.GetAsyncEnumerator();
                if (!await enumerator.MoveNextAsync())
                    Throw.InvalidOperation("sequence contains no elements");

                TSource last;

                do
                {
                    last = enumerator.Current;
                }
                while (await enumerator.MoveNextAsync());

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
                    bool found = false;
                    TSource last = default!;

                    for (var index = 0; index < list.Count; index++)
                    {
                        var item = list[index];
                        if (predicate(item))
                        {
                            last = item;
                            found = true;
                        }
                    }

                    if (!found)
                        Throw.InvalidOperation("sequence contains no matching element");

                    return new ValueTask<TSource>(last);
                }

                Throw.InvalidOperation("sequence contains no elements");
            }

            return LastAsyncFallback(source, predicate);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence, Func<TSource, bool> func)
            {
                TSource last = default!;
                bool found = false;

                await using var enumerator = sequence.GetAsyncEnumerator();
                if (!await enumerator.MoveNextAsync())
                    Throw.InvalidOperation("sequence contains no elements");

                do
                {
                    if (func(enumerator.Current))
                    {
                        found = true;
                        last = enumerator.Current;
                    }
                } 
                while (await enumerator.MoveNextAsync());

                if (!found)
                    Throw.InvalidOperation("sequence contains no matching element");

                return last;
            }
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
                    Throw.InvalidOperation("sequence contains more than 1 element");
                else
                    return ret;
            }

            Throw.InvalidOperation("sequence contains no elements");
            return default;
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
                        Throw.InvalidOperation("sequence contains more than 1 matching element");

                    ret = enumerator.Current;
                    found = true;
                }
            }

            if (!found)
                Throw.InvalidOperation("sequence contains no elements");

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

        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (source is IReadOnlyList<TSource> list)
            {
                if (list.Count > 0)
                {
                    return new ValueTask<TSource>(list[list.Count - 1]);
                }

                return default;
            }

            return LastAsyncFallback(source);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence)
            {
                await using var enumerator = sequence.GetAsyncEnumerator();
                if (!await enumerator.MoveNextAsync())
                    return default!;

                return enumerator.Current;
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
                if (list.Count > 0)
                {
                    bool found = false;
                    TSource last = default!;

                    for (var index = 0; index < list.Count; index++)
                    {
                        var item = list[index];
                        if (predicate(item))
                        {
                            last = item;
                            found = true;
                        }
                    }

                    if (!found)
                        return default;

                    return new ValueTask<TSource>(last);
                }

                return default;
            }

            return LastAsyncFallback(source, predicate);

            static async ValueTask<TSource> LastAsyncFallback(IAsyncEnumerable<TSource> sequence, Func<TSource, bool> func)
            {
                TSource last = default!;
                bool found = false;

                await using var enumerator = sequence.GetAsyncEnumerator();
                if (!await enumerator.MoveNextAsync())
                    return last;

                do
                {
                    if (func(enumerator.Current))
                    {
                        found = true;
                        last = enumerator.Current;
                    }
                }
                while (await enumerator.MoveNextAsync());

                return last;
            }
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
                    Throw.InvalidOperation("sequence contains more than 1 element");
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
                        Throw.InvalidOperation("sequence contains more than 1 matching element");

                    ret = enumerator.Current;
                    found = true;
                }
            }

            return ret;
        }
    }
}

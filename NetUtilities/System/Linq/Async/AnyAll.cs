using NetUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken token = default)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            return source switch
            {
                ICollection<TSource> gCollection => new ValueTask<bool>(gCollection.Count > 0),
                ICollection collection => new ValueTask<bool>(collection.Count > 0),
                _ => AnyAsyncFallback(source, token)
            };

            static async ValueTask<bool> AnyAsyncFallback(IAsyncEnumerable<TSource> sequence, CancellationToken token)
            {
                await using var enumerator = sequence.GetAsyncEnumerator(token);
                return await enumerator.MoveNextAsync();
            }
        }

        public static async ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken token = default)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await using var enumerator = source.GetAsyncEnumerator(token);

            while (await enumerator.MoveNextAsync())
            {
                if (predicate(enumerator.Current))
                    return true;
            }

            return false;
        }

        public static async ValueTask<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken token = default)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await using var enumerator = source.GetAsyncEnumerator(token);

            while (await enumerator.MoveNextAsync())
            {
                if (!predicate(enumerator.Current))
                    return false;
            }

            return true;
        }
    }
}

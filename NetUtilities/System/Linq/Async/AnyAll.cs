using NetUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            return source switch
            {
                ICollection<TSource> gCollection => new ValueTask<bool>(gCollection.Count > 0),
                ICollection collection => new ValueTask<bool>(collection.Count > 0),
                _ => AnyAsyncFallback(source)
            };

            static async ValueTask<bool> AnyAsyncFallback(IAsyncEnumerable<TSource> sequence)
            {
                await using var enumerator = sequence.GetAsyncEnumerator();
                return await enumerator.MoveNextAsync();
            }
        }

        public static async ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        public static async ValueTask<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source is null)
                Throw.NullArgument(nameof(source));

            if (predicate is null)
                Throw.NullArgument(nameof(predicate));

            await foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }
    }
}

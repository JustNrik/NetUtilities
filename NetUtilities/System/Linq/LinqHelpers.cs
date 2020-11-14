using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NetUtilities;

namespace System.Linq
{
    public static class LinqHelpers
    {
        public static IEnumerable<T> Iterate<T>(T seed, Func<T, T> func)
        {
            var next = seed;

            while (true)
                yield return (next = func(next));
        }

        public static IEnumerable<T> Iterate<T>(T seed, int limit, Func<T, T> func)
        {
            var next = seed;
            var index = 0;

            while (index++ < limit)
                yield return (next = func(next));
        }

        public static IEnumerable<T> Iterate<T>(T seed, Func<T, int, T> func)
        {
            var next = seed;
            var index = 0;

            while (true)
                yield return (next = func(next, index++));
        }

        public static IEnumerable<T> Iterate<T>(T seed, int limit, Func<T, int, T> func)
        {
            Ensure.NotOutOfRange(limit >= 0, limit);

            var next = seed;
            var index = 0;

            while (index < limit)
                yield return (next = func(next, index++));
        }

        public static async IAsyncEnumerable<T> IterateAsync<T>(T seed, Func<T, Task<T>> func, [EnumeratorCancellation] CancellationToken token = default)
        {
            var next = seed;

            await new SynchronizationContextRemover();

            while (!token.IsCancellationRequested)
                yield return (next = await func(next));
        }

        public static async IAsyncEnumerable<T> IterateAsync<T>(T seed, Func<T, int, Task<T>> func, [EnumeratorCancellation] CancellationToken token = default)
        {
            var next = seed;
            var index = 0;

            await new SynchronizationContextRemover();

            while (!token.IsCancellationRequested)
                yield return (next = await func(next, index++));
        }
    }
}

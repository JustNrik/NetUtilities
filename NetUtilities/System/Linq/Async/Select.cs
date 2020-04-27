using NetUtilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        /// <inheritdoc cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
        public static IAsyncEnumerable<TTarget> Select<TSource, TTarget>(this IAsyncEnumerable<TSource> source, Func<TSource, TTarget> selector)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(selector);

            return SelectIterator(source, selector);
        }

        public static IAsyncEnumerable<TTarget> Select<TSource, TTarget>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TTarget> selector)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(selector);

            return SelectIndexIterator(source, selector);
        }

        private static async IAsyncEnumerable<TTarget> SelectIterator<TSource, TTarget>(IAsyncEnumerable<TSource> sequence, Func<TSource, TTarget> func)
        {
            await new SynchronizationContextRemover();

            await foreach (var item in sequence)
                yield return func(item);
        }

        private static async IAsyncEnumerable<TTarget> SelectIndexIterator<TSource, TTarget>(IAsyncEnumerable<TSource> sequence, Func<TSource, int, TTarget> func)
        {
            var count = 0;

            await new SynchronizationContextRemover();

            await foreach (var item in sequence)
                yield return func(item, count++);
        }
    }
}

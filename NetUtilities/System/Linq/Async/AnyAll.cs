using NetUtilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Extensions of <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class AsyncEnumerableExtensions
    {
        internal static bool True<T>(T _) => true;

        /// <inheritdoc cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source)
            => AnyAsync(source, True);

        /// <inheritdoc cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            await foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        /// <inheritdoc cref="Enumerable.All{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        public static async ValueTask<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(predicate);

            await new SynchronizationContextRemover();

            await foreach (var item in source)
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }
    }
}

#nullable enable
namespace System.Threading.Tasks
{
    public static partial class TasksUtilities
    {
        /// <summary>
        /// Runs a <see cref="Task{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        public static TResult Run<TResult>(this Task<TResult> task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="Task"/> synchronously.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void Run(this Task task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="ValueTask{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        /// <returns></returns>
        public static TResult Run<TResult>(this ValueTask<TResult> task, bool continueOnCaputedContext = true)
            => task.ConfigureAwait(continueOnCaputedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="ValueTask"/> synchronously.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void Run(this ValueTask task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void RunAsync(this Task task, bool continueOnCapturedContext = false)
            => _ = Task.Run(async () => await task.ConfigureAwait(continueOnCapturedContext));

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void RunAsync<TResult>(this Task<TResult> task, bool continueOnCapturedContext = false)
            => _ = Task.Run(async () => await task.ConfigureAwait(continueOnCapturedContext));

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void RunAsync(this ValueTask task, bool continueOnCapturedContext = false)
            => _ = Task.Run(async () => await task.ConfigureAwait(continueOnCapturedContext));

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="continueOnCapturedContext"></param>
        public static void RunAsync<TResult>(this ValueTask<TResult> task, bool continueOnCapturedContext = false)
            => _ = Task.Run(async () => await task.ConfigureAwait(continueOnCapturedContext));
    }
}

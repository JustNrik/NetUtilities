using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class TasksUtilities
    {
        /// <summary>
        /// Runs a <see cref="Task{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the result object.</typeparam>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        /// <returns>The result of the given task.</returns>
        [return: MaybeNull]
        public static TResult Run<TResult>(this Task<TResult> task, bool continueOnCapturedContext = false)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="Task"/> synchronously.
        /// </summary>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void Run(this Task task, bool continueOnCapturedContext = false)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="ValueTask{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of the result object.</typeparam>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        /// <returns>The result of the given task.</returns>
        [return: MaybeNull]
        public static TResult Run<TResult>(this ValueTask<TResult> task, bool continueOnCaputedContext = false)
            => task.ConfigureAwait(continueOnCaputedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Runs a <see cref="ValueTask"/> synchronously.
        /// </summary>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void Run(this ValueTask task, bool continueOnCapturedContext = false)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void RunAsync(this Task task, bool continueOnCapturedContext = false)
            => _ = task.ConfigureAwait(continueOnCapturedContext);

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <typeparam name="TResult">Type of the result object.</typeparam>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void RunAsync<TResult>(this Task<TResult> task, bool continueOnCapturedContext = false)
            => _ = task.ConfigureAwait(continueOnCapturedContext);

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void RunAsync(this ValueTask task, bool continueOnCapturedContext = false)
            => _ = task.ConfigureAwait(continueOnCapturedContext);

        /// <summary>
        /// Fires and forgets the tasks.
        /// </summary>
        /// <typeparam name="TResult">Type of the result object.</typeparam>
        /// <param name="task">The task that will run.</param>
        /// <param name="continueOnCapturedContext">Indicates if the task should continue in a captured context.</param>
        public static void RunAsync<TResult>(this ValueTask<TResult> task, bool continueOnCapturedContext = false)
            => _ = task.ConfigureAwait(continueOnCapturedContext);
    }
}

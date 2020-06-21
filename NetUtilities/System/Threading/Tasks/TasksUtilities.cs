using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Threading.Tasks
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class TasksUtilities
    {
        /// <summary>
        ///     Runs a <see cref="Task{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of the result object.
        /// </typeparam>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <returns>
        ///     The result of the given task.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static TResult Run<TResult>(this Task<TResult> task)
            => Ensure.NotNull(task).GetAwaiter().GetResult();

        /// <summary>
        ///     Runs a <see cref="Task"/> synchronously.
        /// </summary>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static void Run(this Task task)
            => Ensure.NotNull(task).GetAwaiter().GetResult();

        /// <summary>
        ///     Runs a <see cref="ValueTask{TResult}"/> synchronously and returns <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of the result object.
        /// </typeparam>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <returns>
        ///     The result of the given task.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static TResult Run<TResult>(this ValueTask<TResult> task)
            => Ensure.NotNull(task).GetAwaiter().GetResult();

        /// <summary>
        ///     Runs a <see cref="ValueTask"/> synchronously.
        /// </summary>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static void Run(this ValueTask task)
            => Ensure.NotNull(task).GetAwaiter().GetResult();

        /// <summary>
        ///     Fires and forgets the tasks.
        /// </summary>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static void RunAsync(this Task task)
            => SynchronizationContextHelper.Run(async (task) => await task, Ensure.NotNull(task));

        /// <summary>
        ///     Fires and forgets the tasks.
        /// </summary>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static void RunAsync(this ValueTask task)
            => SynchronizationContextHelper.Run(async (task) => await task, Ensure.NotNull(task));

        /// <summary>
        ///     Fires and forgets the tasks.
        /// </summary>
        /// <typeparam name="TResult">
        ///     Type of the result object.
        /// </typeparam>
        /// <param name="task">
        ///     The task that will run.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the task is <see langword="null"/>.
        /// </exception>
        public static void RunAsync<TResult>(this ValueTask<TResult> task)
            => SynchronizationContextHelper.Run(async (task) => await task, Ensure.NotNull(task));
    }
}

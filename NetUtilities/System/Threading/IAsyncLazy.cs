using System.Runtime.CompilerServices;

namespace System.Threading
{
    /// <summary>
    ///     Provides support for lazy asynchronous initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public interface IAsyncLazy<T> : ILazy<T>
    {
        /// <summary>
        ///     Starts the underlying <see cref="Tasks.Task{TResult}"/> if <see cref="Tasks.Task.Status"/> is 
        ///     <see cref="Tasks.TaskStatus.Created"/>. This method also calls the underlying factory method if
        ///     the task hasn't been allocated yet and starts the task.
        /// </summary>
        void StartTask();
        /// <summary>
        ///     Gets the underlaying <see cref="Tasks.Task{TResult}"/>'s <see cref="TaskAwaiter{TResult}"/>.
        /// </summary>
        /// <returns>
        ///     The <see cref="TaskAwaiter{TResult}"/> of the underlying task.
        /// </returns>
        TaskAwaiter<T> GetAwaiter();
    }
}

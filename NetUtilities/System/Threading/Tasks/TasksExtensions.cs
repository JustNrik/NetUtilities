using System.ComponentModel;

namespace System.Threading.Tasks
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class TasksExtensions
    {
        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard(this Task _)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard(this ValueTask _)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard<T>(this ValueTask<T> _)
        {
        }

        public static async Task<TResult> Then<TResult>(this Task task, Task<TResult> next)
        {
            await task;
            return await next;
        }

        public static async Task Then(this Task task, Func<Task> next)
        {
            await task;
            await next();
        }

        public static async Task<TResult> Then<TResult>(this Task task, Func<Task<TResult>> next)
        {
            await task;
            return await next();
        }

        public static async Task Then(this Task task, Action next)
        {
            await task;
            next();
        }

        public static async Task Then<TState>(this Task task, Action<TState> next, TState state)
        {
            await task;
            next(state);
        }

        public static async Task<TResult> Then<TResult>(this Task task, Func<TResult> next)
        {
            await task;
            return next();
        }

        public static async Task Then<TState>(this Task task, Func<TState, Task> next, TState state)
        {
            await task;
            await next(state);
        }

        public static async Task<TResult> Then<TState, TResult>(this Task task, Func<TState, TResult> next, TState state)
        {
            await task;
            return next(state);
        }

        public static async Task<TResult> Then<TState, TResult>(this Task task, Func<TState, Task<TResult>> next, TState state)
        {
            await task;
            return await next(state);
        }

        public static async Task Then<T>(this Task<T> task, Func<T, Task> next)
            => await next(await task);

        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, Task<TResult>> next)
            => await next(await task);

        public static async Task Then<T>(this Task<T> task, Action<T> next)
            => next(await task);

        public static async Task Then<T, TState>(this Task<T> task, Action<T, TState> next, TState state)
            => next(await task, state);

        public static async Task<TResult> Then<T, TResult>(this Task<T> task, Func<T, TResult> next)
            => next(await task);

        public static async Task<TResult> Then<T, TState, TResult>(this Task<T> task, Func<T, TState, TResult> next, TState state)
            => next(await task, state);

        public static async Task Then<T, TState>(this Task<T> task, Func<T, TState, Task> next, TState state)
            => await next(await task, state);

        public static async Task<TResult> Then<T, TState, TResult>(this Task<T> task, Func<T, TState, Task<TResult>> next, TState state)
            => await next(await task, state);

        public static async ValueTask<TResult> Then<TResult>(this ValueTask ValueTask, ValueTask<TResult> next)
        {
            await ValueTask;
            return await next;
        }

        public static async ValueTask Then(this ValueTask ValueTask, Func<ValueTask> next)
        {
            await ValueTask;
            await next();
        }

        public static async ValueTask<TResult> Then<TResult>(this ValueTask ValueTask, Func<ValueTask<TResult>> next)
        {
            await ValueTask;
            return await next();
        }

        public static async ValueTask Then(this ValueTask ValueTask, Action next)
        {
            await ValueTask;
            next();
        }

        public static async ValueTask Then<TState>(this ValueTask ValueTask, Action<TState> next, TState state)
        {
            await ValueTask;
            next(state);
        }

        public static async ValueTask<TResult> Then<TResult>(this ValueTask ValueTask, Func<TResult> next)
        {
            await ValueTask;
            return next();
        }

        public static async ValueTask Then<TState>(this ValueTask ValueTask, Func<TState, ValueTask> next, TState state)
        {
            await ValueTask;
            await next(state);
        }

        public static async ValueTask<TResult> Then<TState, TResult>(this ValueTask ValueTask, Func<TState, TResult> next, TState state)
        {
            await ValueTask;
            return next(state);
        }

        public static async ValueTask<TResult> Then<TState, TResult>(this ValueTask ValueTask, Func<TState, ValueTask<TResult>> next, TState state)
        {
            await ValueTask;
            return await next(state);
        }

        public static async ValueTask Then<T>(this ValueTask<T> ValueTask, Func<T, ValueTask> next)
            => await next(await ValueTask);

        public static async ValueTask<TResult> Then<T, TResult>(this ValueTask<T> ValueTask, Func<T, ValueTask<TResult>> next)
            => await next(await ValueTask);

        public static async ValueTask Then<T>(this ValueTask<T> ValueTask, Action<T> next)
            => next(await ValueTask);

        public static async ValueTask Then<T, TState>(this ValueTask<T> ValueTask, Action<T, TState> next, TState state)
            => next(await ValueTask, state);

        public static async ValueTask<TResult> Then<T, TResult>(this ValueTask<T> ValueTask, Func<T, TResult> next)
            => next(await ValueTask);

        public static async ValueTask<TResult> Then<T, TState, TResult>(this ValueTask<T> ValueTask, Func<T, TState, TResult> next, TState state)
            => next(await ValueTask, state);

        public static async ValueTask Then<T, TState>(this ValueTask<T> ValueTask, Func<T, TState, ValueTask> next, TState state)
            => await next(await ValueTask, state);

        public static async ValueTask<TResult> Then<T, TState, TResult>(this ValueTask<T> ValueTask, Func<T, TState, ValueTask<TResult>> next, TState state)
            => await next(await ValueTask, state);
    }
}

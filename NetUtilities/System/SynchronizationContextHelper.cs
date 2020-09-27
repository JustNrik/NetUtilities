using System.Threading;
using System.Threading.Tasks;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     This class is a helper to run an action in the <see cref="DefaultSynchronizationContext"/>. 
    /// </summary>
    public static class SynchronizationContextHelper
    {
        private static EventHandler<DefaultSynchronizationContext, CapturedExceptionEventArgs>? _capturedException;
        private static readonly object _lock = new object();

        internal static void OnCapturedException(DefaultSynchronizationContext context, CapturedExceptionEventArgs eventArgs)
            => _capturedException?.Invoke(context, eventArgs);

        /// <summary>
        ///     Occurs when exceptions are captured in the <see cref="DefaultSynchronizationContext"/>.
        /// </summary>
        public static event EventHandler<DefaultSynchronizationContext, CapturedExceptionEventArgs>? CapturedException
        {
            add
            {
                lock (_lock)
                    _capturedException += value;
            }
            remove
            {
                lock (_lock)
                    _capturedException -= value;
            }
        }

        /// <summary>
        ///     Runs the <paramref name="action"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="action">
        ///     The action to execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the action is <see langword="null"/>.
        /// </exception>
        public static void Run(Action action)
        {
            Ensure.NotNull(action);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        ///     Runs the <paramref name="action"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance 
        ///     with the arguments provided via <paramref name="state"/>.
        /// </summary>
        /// <typeparam name="TState">
        ///     The type of the arguments for the action.
        /// </typeparam>
        /// <param name="action">
        ///     The action to execute.
        /// </param>
        /// <param name="state">
        ///     The arguments for the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when action or state are <see langword="null"/>.
        /// </exception>
        public static void Run<TState>(Action<TState> action, TState state)
        {
            Ensure.NotNull(action);
            Ensure.NotNull(state);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                action(state);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        ///     Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">
        ///     The function to execute.
        /// </param>
        /// <returns>
        ///     The task object returned by the func.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the func is <see langword="null"/>.
        /// </exception>
        public static Task RunAsync(Func<Task> func)
        {
            Ensure.NotNull(func);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                return func();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        ///     Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <typeparam name="TState">
        ///     The type of the arguments for the action.
        /// </typeparam>
        /// <param name="func">
        ///     The function to execute.
        /// </param>
        /// <param name="state">
        ///     The arguments for the action.
        /// </param>
        /// <returns>
        ///     The task object returned by the func.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when func or state are <see langword="null"/>.
        /// </exception>
        public static Task RunAsync<TState>(Func<TState, Task> func, TState state)
        {
            Ensure.NotNull(func);
            Ensure.NotNull(state);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                return func(state);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        ///     Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">
        ///     The function to execute.
        /// </param>
        /// <typeparam name="TResult">
        ///     The result of the task.
        /// </typeparam>
        /// <returns>
        ///     The task object returned by the func.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the func is <see langword="null"/>.
        /// </exception>
        public static Task<TResult> RunAsync<TResult>(Func<Task<TResult>> func)
        {
            Ensure.NotNull(func);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                return func();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        /// <summary>
        ///     Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <typeparam name="TState">
        ///     The type of the arguments for the action.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The result of the task.
        /// </typeparam>
        /// <param name="func">
        ///     The function to execute.
        /// </param>
        /// <param name="state">
        ///     The object to be passed to the delegate.
        /// </param>
        /// <returns>
        ///     The task object returned by the func.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when func or state are <see langword="null"/>.
        /// </exception>
        public static Task<TResult> RunAsync<TState, TResult>(Func<TState, Task<TResult>> func, TState state)
        {
            Ensure.NotNull(func);
            Ensure.NotNull(state);

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(DefaultSynchronizationContext.Shared);
                return func(state);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }
    }
}

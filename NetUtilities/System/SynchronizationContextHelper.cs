using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// This class is a helper to run an action in the <see cref="DefaultSynchronizationContext"/>. 
    /// </summary>
    public static class SynchronizationContextHelper
    {
        private static EventHandler<DefaultSynchronizationContext, CapturedExceptionEventArgs>? _capturedException;
        private static readonly object _lock = new object();

        internal static void OnCapturedException(DefaultSynchronizationContext context, CapturedExceptionEventArgs eventArgs)
            => _capturedException?.Invoke(context, eventArgs);

        /// <summary>
        /// Occurs when exceptions are captured in the <see cref="DefaultSynchronizationContext"/>.
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
        /// Runs the <paramref name="action"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void Run(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

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
        /// Runs the <paramref name="action"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance with the arguments provided via <paramref name="state"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="state">The arguments for the action.</param>
        /// <typeparam name="TState">The type of the arguments for the action.</typeparam>
        public static void Run<TState>(Action<TState> action, TState state)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

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
        /// Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <returns>The task object returned by the func.</returns>
        public static Task RunAsync(Func<Task> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

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
        /// Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <param name="state">The arguments for the action.</param>
        /// <typeparam name="TState">The type of the arguments for the action.</typeparam>
        /// <returns>The task object returned by the func.</returns>
        public static Task RunAsync<TState>(Func<TState, Task> func, TState state)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

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
        /// Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <typeparam name="TResult">The result of the task.</typeparam>
        /// <returns>The task object returned by the func.</returns>
        public static Task<TResult> RunAsync<TResult>(Func<Task<TResult>> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

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
        /// Runs the <paramref name="func"/> in the <see cref="DefaultSynchronizationContext.Shared"/> instance.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <param name="state">The object to be passed to the delegate.</param>
        /// <typeparam name="TState">The type of the arguments for the action.</typeparam>
        /// <typeparam name="TResult">The result of the task.</typeparam>
        /// <returns>The task object returned by the func.</returns>
        public static Task<TResult> RunAsync<TState, TResult>(Func<TState, Task<TResult>> func, TState state)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));

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

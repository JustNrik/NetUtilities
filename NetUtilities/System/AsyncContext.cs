using System.Threading;

namespace System
{
    /// <summary>
    /// This class is a helper to run an action in the <see cref="DefaultSynchronizationContext"/>. 
    /// This class is designed for <see langword="async"/> <see langword="void"/> methods.
    /// </summary>
    public static class AsyncContext
    {
        /// <summary>
        /// This event raises when exceptions are captured when <see cref="Run(Action)"/> invokes the action.
        /// </summary>
        public static event EventHandler<DefaultSynchronizationContext, CapturedExceptionEventArgs>? CapturedException
        {
            add
            {
                lock (_lock)
                    AsyncContextInvoker.CapturedException += value;
            }
            remove
            {
                lock (_lock)
                    AsyncContextInvoker.CapturedException -= value;
            }
        }

        private static readonly object _lock = new object();

        /// <summary>
        /// Runs the <paramref name="action"/> in the <see cref="DefaultSynchronizationContext"/>.
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            var previousContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(new DefaultSynchronizationContext());
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

    }
}

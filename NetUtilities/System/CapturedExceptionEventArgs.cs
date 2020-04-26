namespace System
{
    /// <summary>
    /// This class contains the <see cref="Exception"/> throw on <see cref="DefaultSynchronizationContext"/>. 
    /// If multiple exceptions are throw, they will be wrapped on an <see cref="AggregateException"/>.
    /// </summary>
    public class CapturedExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// The exception.
        /// </summary>
        public Exception Exception { get; }

        internal CapturedExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}

namespace System
{
    /// <summary>
    /// This class contains the <see cref="Exception"/> throw on <see cref="AsyncContext.Run(Action)"/>. 
    /// If multiple exceptions are throw, they will be wrapped on a <see cref="AggregateException"/>.
    /// </summary>
    public class CapturedExceptionEventArgs
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

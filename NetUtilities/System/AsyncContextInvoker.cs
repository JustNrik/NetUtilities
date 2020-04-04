namespace System
{
    internal static class AsyncContextInvoker
    {
        public static event EventHandler<DefaultSynchronizationContext, CapturedExceptionEventArgs>? CapturedException;

        public static void OnCapturedException(DefaultSynchronizationContext context, CapturedExceptionEventArgs eventArgs)
        {
            CapturedException?.Invoke(context, eventArgs);
        }
    }
}

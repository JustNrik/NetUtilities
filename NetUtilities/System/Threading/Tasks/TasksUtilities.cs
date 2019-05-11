namespace System.Threading.Tasks
{
    public static partial class TasksUtilities
    {
        public static TResult AwaitResult<TResult>(this Task<TResult> task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
        public static void Await(this Task task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
        public static TResult AwaitResult<TResult>(this ValueTask<TResult> task, bool continueOnCaputedContext = true)
            => task.ConfigureAwait(continueOnCaputedContext).GetAwaiter().GetResult();
        public static void Await(this ValueTask task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
    }
}

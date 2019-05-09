namespace System.Threading.Tasks
{
    public static partial class TasksUtilities
    {
        public static TResult AwaitResult<TResult>(this Task<TResult> task, bool continueOnCapturedContext = true) 
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
        public static void Await(this Task task, bool continueOnCapturedContext = true) 
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
    }
}

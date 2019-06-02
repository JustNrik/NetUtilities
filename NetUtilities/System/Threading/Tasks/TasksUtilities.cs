#nullable enable
namespace System.Threading.Tasks
{
    public static partial class TasksUtilities
    {
        public static TResult Run<TResult>(this Task<TResult> task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
        public static void Run(this Task task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
        public static TResult Run<TResult>(this ValueTask<TResult> task, bool continueOnCaputedContext = true)
            => task.ConfigureAwait(continueOnCaputedContext).GetAwaiter().GetResult();
        public static void Run(this ValueTask task, bool continueOnCapturedContext = true)
            => task.ConfigureAwait(continueOnCapturedContext).GetAwaiter().GetResult();
    }
}

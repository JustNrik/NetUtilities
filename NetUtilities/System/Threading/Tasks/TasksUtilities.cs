namespace System.Threading.Tasks
{
    public static partial class TasksUtilities
    {
        public static void NotAwait(this Task task) { }
        public static TResult Await<TResult>(this Task<TResult> task) => task.GetAwaiter().GetResult();
        public static void Await(this Task task) => task.GetAwaiter().GetResult();
    }
}

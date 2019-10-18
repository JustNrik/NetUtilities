using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// This structure is used to remove the current SynchronizationContext. This helps to reduce the verbosity of using <see cref="Task.ConfigureAwait(bool)"/>
    /// </summary>
    public struct SynchronizationContextRemover : INotifyCompletion
    {
        public bool IsCompleted 
            => SynchronizationContext.Current is null;

        public void OnCompleted(Action action)
        {
            var previous = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previous);
            }
        }

        public SynchronizationContextRemover GetAwaiter()
            => this;

        public void GetResult() { }
    }
}

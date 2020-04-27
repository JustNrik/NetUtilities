using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// This structure is used to remove the current SynchronizationContext. This helps to reduce the verbosity of using <see cref="Task.ConfigureAwait(bool)"/>
    /// </summary>
    public struct SynchronizationContextRemover : INotifyCompletion
    {
        /// <summary>
        /// Returns if <see cref="SynchronizationContext.Current"/> is <see langword="null"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsCompleted 
            => SynchronizationContext.Current is null;

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
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

        /// <summary>
        /// Returns <see langword="this"/>.
        /// </summary>
        /// <returns><see langword="this"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SynchronizationContextRemover GetAwaiter()
            => this;

        /// <summary>
        /// This method does nothing.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetResult() 
        {
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using NetUtilities;

namespace System.Threading.Tasks
{
    /// <summary>
    ///     This structure is used to remove <see cref="SynchronizationContext.Current"/>. 
    ///     This helps to reduce the verbosity of using <see cref="Task.ConfigureAwait(bool)"/>
    /// </summary>
    public readonly struct SynchronizationContextRemover : INotifyCompletion
    {
        /// <summary>
        ///     Indicates if <see cref="SynchronizationContext.Current"/> is <see langword="null"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsCompleted
            => SynchronizationContext.Current is null;

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnCompleted(Action action)
        {
            Ensure.NotNull(action);

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
        ///     Returns the current instace of <see cref="SynchronizationContextRemover"/>.
        /// </summary>
        /// <returns>
        ///     The current instace of <see cref="SynchronizationContextRemover"/>.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SynchronizationContextRemover GetAwaiter()
            => this;

        /// <summary>
        ///     This method does nothing.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetResult()
        {
        }
    }
}

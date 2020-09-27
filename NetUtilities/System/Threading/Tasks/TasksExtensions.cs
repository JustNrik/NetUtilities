using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Threading.Tasks
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class TasksExtensions
    {
        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard(this Task task)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard(this ValueTask task)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard<T>(this ValueTask<T> task)
        {
        }
    }
}

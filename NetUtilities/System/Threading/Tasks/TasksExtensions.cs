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
        public static void Discard(this Task _)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard(this ValueTask _)
        {
        }

        /// <summary>
        ///     Fires and forgets the task.
        /// </summary>
        /// <param name="_">
        ///     The task to discard.
        /// </param>
        public static void Discard<T>(this ValueTask<T> _)
        {
        }
    }
}

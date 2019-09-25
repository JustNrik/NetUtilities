using NetUtilities;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Invokes an action for each element in the sequence.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either the source or the action are null.</exception>
        /// <typeparam name="T">The underlying type of the sequence.</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="action">The action to be executed on each item of the sequence.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(action, nameof(action));

            foreach (T element in source)
                action(element);
        }
    }
}

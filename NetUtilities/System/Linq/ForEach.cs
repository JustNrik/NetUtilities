using NetUtilities;
using System.Collections.Generic;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        /// <summary>
        /// Invokes an action for each element in the sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(action, nameof(action));

            foreach (T element in source)
                action(element);
        }
    }
}

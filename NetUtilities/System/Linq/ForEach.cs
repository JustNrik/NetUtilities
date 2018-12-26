using System.Collections.Generic;

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
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (action is null) throw new ArgumentNullException(nameof(action));

            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}

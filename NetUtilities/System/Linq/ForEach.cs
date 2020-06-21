using System.Collections.Generic;
using System.Net.NetworkInformation;
using NetUtilities;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        ///     Invokes an action for each element in the sequence.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the sequence.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence.
        /// </param>
        /// <param name="action">
        ///     The action to be executed on each item of the sequence.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either the source or the action are <see langword="null"/>.
        /// </exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(action);

            foreach (var element in source)
                action(element);
        }

        /// <summary>
        ///     Invokes an action for each element in the sequence with the provided parameters.
        /// </summary>
        /// <typeparam name="TState">
        ///     The type of the parameters to be passed.
        /// </typeparam>
        /// <typeparam name="T">
        ///     The underlying type of the sequence.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence.
        /// </param>
        /// <param name="action">
        ///     The action delegate to be executed on each item of the sequence.
        /// </param>
        /// <param name="state">
        ///     The arguments to be passed to the action delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either the source or the action are <see langword="null"/>.
        /// </exception>
        public static void ForEach<TState, T>(this IEnumerable<T> source, TState state, Action<TState, T> action)
        {
            Ensure.NotNull(source);
            Ensure.NotNull(action);

            foreach (var element in source)
                action(state, element);
        }
    }
}

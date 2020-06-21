using System.Threading;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     Represets a local action, which will be executed instanly and undone when the scope ends.
    /// </summary>
    public readonly ref struct ActionScope
    {
        private readonly Action _undoAction;
        private readonly CancellationTokenSource _cts;

        /// <summary>
        ///     Creates an <see cref="ActionScope"/> with the provided starting action and undo action.
        /// </summary>
        /// <param name="startAction">
        ///     The starting action.
        /// </param>
        /// <param name="undoAction">
        ///     The undo action.
        /// </param>
        /// <returns>
        ///     An <see cref="ActionScope"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="startAction"/> or <paramref name="undoAction"/> are <see langword="null"/>.
        /// </exception>
        public static ActionScope Create(Action startAction, Action undoAction)
            => new ActionScope(startAction, undoAction);

        private ActionScope(Action startAction, Action undoAction)
        {
            Ensure.NotNull(startAction);
            Ensure.NotNull(undoAction);

            _undoAction = undoAction;
            _cts = new CancellationTokenSource();
            startAction.Invoke();
        }

        /// <summary>
        ///     Prevents the undo action to be executed.
        /// </summary>
        public void Cancel()
            => _cts.Cancel();

        /// <summary>
        ///     Executes the undo action and disposes the internal <see cref="CancellationTokenSource"/>.
        /// </summary>
        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction.Invoke();

            _cts.Dispose();
        }
    }

    /// <summary>
    ///     Represets a local action, which will be executed instanly and undone when the scope ends.
    /// </summary>
    /// <typeparam name="TState">
    ///     The arguments to be used in the scope.
    /// </typeparam>
    public readonly ref struct ActionScope<TState> where TState : notnull
    {
        private readonly Action<TState> _undoAction;
        private readonly CancellationTokenSource _cts;
        private readonly TState _state;

        /// <summary>
        ///     Creates an <see cref="ActionScope"/> with the provided starting action and undo action.
        /// </summary>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="startAction">
        ///     The starting action.
        /// </param>
        /// <param name="undoAction">
        ///     The undo action.
        /// </param>
        /// <returns>
        ///     An <see cref="ActionScope{TState}"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="state"/>, <paramref name="startAction"/> or <paramref name="undoAction"/> are <see langword="null"/>.
        /// </exception>
        public static ActionScope<TState> Create(TState state, Action<TState> startAction, Action<TState> undoAction)
            => new ActionScope<TState>(state, startAction, undoAction);

        private ActionScope(TState state, Action<TState> startAction, Action<TState> undoAction)
        {
            Ensure.NotNull(state);
            Ensure.NotNull(startAction);
            Ensure.NotNull(undoAction);

            _undoAction = undoAction;
            _state = state;
            _cts = new CancellationTokenSource();
            startAction.Invoke(state);
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction.Invoke(_state);

            _cts.Dispose();
        }
    }
}
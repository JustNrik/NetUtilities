using System.Threading.Tasks.Sources;

namespace System.Threading
{
    /// <summary>
    ///     Wrapper class of <see cref="ManualResetValueTaskSourceCore{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">
    ///     The result type.
    /// </typeparam>
    public sealed class ManualResetValueTaskSource<TResult> : IValueTaskSource<TResult>, IValueTaskSource
    {
        private ManualResetValueTaskSourceCore<TResult> _core;

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.RunContinuationsAsynchronously"/>
        public bool RunContinuationsAsynchronously
        {
            get => _core.RunContinuationsAsynchronously;
            set => _core.RunContinuationsAsynchronously = value;
        }

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.Version"/>
        public short Version
            => _core.Version;

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.Reset"/>
        public void Reset()
            => _core.Reset();

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.SetResult(TResult)"/>
        public void SetResult(TResult result) 
            => _core.SetResult(result);

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.SetException(Exception)"/>
        public void SetException(Exception error) 
            => _core.SetException(error);

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.GetResult(short)"/>
        public TResult GetResult(short token)
            => _core.GetResult(token);

        public TResult UnsafeGetResult()
            => _core.GetResult(_core.Version);

        void IValueTaskSource.GetResult(short token)
            => _core.GetResult(token);

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.GetStatus(short)"/>
        public ValueTaskSourceStatus GetStatus(short token)
            => _core.GetStatus(token);

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.OnCompleted(Action{object?}, object?, short, ValueTaskSourceOnCompletedFlags)"/>
        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
            => _core.OnCompleted(continuation, state, token, flags);

        /// <inheritdoc cref="ManualResetValueTaskSourceCore{TResult}.GetStatus(short)"/>
        public ValueTaskSourceStatus GetStatus()
            => _core.GetStatus(_core.Version);

        public bool IsCompleted
            => _core.GetStatus(_core.Version) != ValueTaskSourceStatus.Pending;

        public bool IsCompletedSuccessfully
            => _core.GetStatus(_core.Version) == ValueTaskSourceStatus.Succeeded;

        public void TrySetResult(TResult result)
        {
            if (_core.GetStatus(_core.Version) == ValueTaskSourceStatus.Pending)
                _core.SetResult(result);
        }

        public ValueTask<TResult> AsValueTask()
            => new(this, Version);

        public Task<TResult> AsTask()
            => AsValueTask().AsTask();
    }
}
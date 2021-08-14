using System.Runtime.CompilerServices;
using NetUtilities;

namespace System.Threading
{
    /// <inheritdoc cref="IAsyncLazy{T}"/>
    public sealed class AsyncLazy<T, TState> : IAsyncLazy<T>
    {
        private const int NotStarted = 0;
        private const int Running = 1;

        private volatile int _status;

        private readonly ManualResetValueTaskSource<T> _promise;
        private TState? _state;
        private Func<TState, Task<T>>? _factory;

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the value hasn't been created yet. 
        ///     It's recommended to access this property after checking if
        ///     <see cref="IsValueCreated"/> returns <see langword="true"/>.
        /// </exception>
        public T Value
        {
            get
            {
                if (IsValueCreated)
                    return _promise.UnsafeGetResult();

                throw new InvalidOperationException("The value is not created yet.");
            }
        }

        /// <inheritdoc/>
        public bool IsValueCreated
            => _promise.IsCompletedSuccessfully;

        private AsyncLazy()
        {
            _promise = new();
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        public AsyncLazy(T value) : this()
        {
            _promise.SetResult(value);
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class wit the provided
        ///     factory delegate.
        /// </summary>
        /// <param name="factory">
        ///     The factory delegate.
        /// </param>
        public AsyncLazy(Func<TState, Task<T>> factory, TState state) : this()
        { 
            _factory = Ensure.NotNull(factory);
            _state = Ensure.NotNull(state);
        }

        private async void RunTask(Task<T> task)
        {
            if (Interlocked.CompareExchange(ref _status, Running, NotStarted) == Running)
                return;

            try
            {
                var result = await task;
                _promise.TrySetResult(result);
            }
            catch (Exception e)
            {
                _promise.SetException(e);
            }
        }

        /// <inheritdoc/>
        public void StartTask()
        {
            if (IsValueCreated)
                return;

            var factory = Interlocked.Exchange(ref _factory, null);

            if (factory is null)
                return;

            var task = factory(_state!);
            _state = default;

            if (task is null)
                throw new Exception("The task factory returned a null task.");

            RunTask(task);
        }

        /// <inheritdoc/>
        public ValueTaskAwaiter<T> GetAwaiter()
        {
            if (IsValueCreated)
                return new ValueTask<T>(_promise.UnsafeGetResult()).GetAwaiter();

            StartTask();
            return new ValueTask<T>(_promise, _promise.Version).GetAwaiter();
        }
    }
}

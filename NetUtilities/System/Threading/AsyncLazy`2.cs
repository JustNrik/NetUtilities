using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NetUtilities;

namespace System.Threading
{
    /// <inheritdoc cref="IAsyncLazy{T}"/>
    public sealed class AsyncLazy<T, TState> : IAsyncLazy<T>
    {
        private T? _value;
        private TState? _state;
        private Task<T>? _task;
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
                    return _value;

                throw new InvalidOperationException("The value is not created yet.");
            }
        }

        [MemberNotNullWhen(true, nameof(_value))]
        [MemberNotNullWhen(false, nameof(_task))]
        [MemberNotNullWhen(false, nameof(_state))]
        public bool IsValueCreated { get; private set; }

        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        public AsyncLazy(T value)
        {
            _value = value;

            IsValueCreated = true;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class wit the provided
        ///     factory delegate.
        /// </summary>
        /// <param name="factory">
        ///     The factory delegate.
        /// </param>
        public AsyncLazy(Func<TState, Task<T>> factory, TState state)
        {
            Ensure.NotNull(factory);
            Ensure.NotNull(state);

            _factory = factory;
            _state = state;
        }

        private static T Continuation(Task<T> task, object? obj)
        {
            var lazy = (AsyncLazy<T, TState>)obj!;
            lazy.IsValueCreated = true;
            lazy._value = task.GetAwaiter().GetResult();
            lazy._task = null;
            return lazy._value;
        }

        public void StartTask()
        {
            if (IsValueCreated)
                return;

            if (_factory is not null)
            {
                _task = _factory(_state);
                _factory = null;
                _state = default;

                if (_task.IsCompleted) // could have completed synchronously
                {
                    var value = _task.GetAwaiter().GetResult(); // unwrapping exceptions

                    IsValueCreated = true;
                    _value = value;
                    _task = null;
                    return;
                }
            }

            if (_task.Status == TaskStatus.Created)
                _task.Start();

            _task = _task.ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously);
        }

        public ValueTaskAwaiter<T> GetAwaiter()
        {
            if (IsValueCreated)
                return new ValueTask<T>(_value).GetAwaiter();

            if (_factory is not null)
            {
                _task = _factory(_state);
                _factory = null;
                _state = default;
            }

            if (_task.Status == TaskStatus.Created)
                _task.Start();

            _task = _task.ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously);
            return new ValueTask<T>(_task).GetAwaiter();
        }
    }
}

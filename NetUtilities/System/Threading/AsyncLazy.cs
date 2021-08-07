using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NetUtilities;

namespace System.Threading
{
    /// <inheritdoc cref="IAsyncLazy{T}"/>
    public sealed class AsyncLazy<T> : IAsyncLazy<T>
    {
        private T? _value;
        private Task<T>? _task;
        private Func<Task<T>>? _factory;

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
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class with the provided
        ///     <see cref="Task{TResult}"/>.
        /// </summary>
        /// <param name="task">
        ///     The task.
        /// </param>
        public AsyncLazy(Task<T> task)
        {
            Ensure.NotNull(task);

            if (task.IsCompleted)
            {
                _value = task.GetAwaiter().GetResult(); // unwrapping exceptions
                IsValueCreated = true;
            }

            _task = task.ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="AsyncLazy{T}"/> class wit the provided
        ///     factory delegate.
        /// </summary>
        /// <param name="factory">
        ///     The factory delegate.
        /// </param>
        public AsyncLazy(Func<Task<T>> factory)
        {
            Ensure.NotNull(factory);

            _factory = factory;
        }

        private static T Continuation(Task<T> task, object? obj)
        {
            var lazy = (AsyncLazy<T>)obj!;
            lazy.IsValueCreated = true;
            lazy._value = task.GetAwaiter().GetResult();
            return lazy._value;
        }

        public void StartTask()
        {
            if (IsValueCreated)
                return;

            if (_factory is not null)
            {
                _task = _factory().ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously);
                _factory = null;

                if (_task.IsCompleted) // could have completed synchronously
                {
                    var value = _task.GetAwaiter().GetResult(); // unwrapping exceptions

                    IsValueCreated = true;
                    _value = value;
                    return;
                }
            }

            if (_task is { Status: TaskStatus.Created })
                _task.Start();
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            if (IsValueCreated)
                return Task.FromResult(_value).GetAwaiter();

            if (_factory is not null)
            {
                _task = _factory().ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously);
                _factory = null;
            }

            return _task!.GetAwaiter();
        }
    }

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

            _factory = factory;
            _state = state;
        }

        private static T Continuation(Task<T> task, object? obj)
        {
            var lazy = (AsyncLazy<T, TState>)obj!;
            lazy.IsValueCreated = true;
            lazy._value = task.GetAwaiter().GetResult();
            return lazy._value;
        }

        public void StartTask()
        {
            if (IsValueCreated)
                return;

            if (_factory is not null)
            {
                _task = _factory(_state!).ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously); ;
                _factory = null;
                _state = default;

                if (_task.IsCompleted) // could have completed synchronously
                {
                    var value = _task.GetAwaiter().GetResult(); // unwrapping exceptions

                    IsValueCreated = true;
                    _value = value;
                    return;
                }
            }

            if (_task is { Status: TaskStatus.Created })
                _task.Start();
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            if (IsValueCreated)
                return Task.FromResult(_value).GetAwaiter();

            if (_factory is not null)
            {
                _task = _factory(_state!).ContinueWith(Continuation, this, TaskContinuationOptions.ExecuteSynchronously); ;
                _factory = null;
                _state = default;
            }

            return _task!.GetAwaiter();
        }
    }
}

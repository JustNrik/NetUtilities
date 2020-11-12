using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     A lightweight implementation of <see cref="Lazy{T}"/>, which also provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public sealed class SlimLazy<T> : ILazy<T>
    {
        private T? _value;
        private Func<T>? _valueFactory;
        private readonly object? _lock;

        /// <inheritdoc/>
        public T Value
        {
            get
            {
                if (_lock is not null)
                {
                    lock (_lock)
                    {
                        if (!IsInitialized)
                        {
                            _value = _valueFactory();
                            _valueFactory = null;
                            IsInitialized = true;
                        }
                    }
                }
                else
                {
                    if (!IsInitialized)
                    {
                        _value = _valueFactory();
                        _valueFactory = null;
                        IsInitialized = true;
                    }
                }

                return _value;
            }
        }

        /// <inheritdoc/>
        [MemberNotNullWhen(false, nameof(_valueFactory))]
        [MemberNotNullWhen(true, nameof(_value))]
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SlimLazy{T}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        public SlimLazy(T value)
        {
            _value = value;
            IsInitialized = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SlimLazy{T}"/> class with the provided factory delegate. 
        ///     Optionally you can indicate if the initialization should be tread-safe.
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <param name="threadSafe">
        ///     Indicates if the initialization should be thread-safe.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="valueFactory"/> is <see langword="null"/>.
        /// </exception>
        public SlimLazy(Func<T> valueFactory, bool threadSafe = false)
        {
            _valueFactory = Ensure.NotNull(valueFactory);

            if (threadSafe)
                _lock = new object();
        }
    }

    /// <summary>
    ///     A lightweight implementation of <see cref="Lazy{T, TMetadata}"/>, which also provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    /// <typeparam name="TState">
    ///     The state.
    /// </typeparam>
    public sealed class SlimLazy<T, TState> : ILazy<T>
    {
        private T? _value;
        private Func<TState, T>? _valueFactory;
        private readonly TState? _state;
        private readonly object? _lock;

        /// <inheritdoc/>
        public T Value
        {
            get
            {
                if (_lock is not null)
                {
                    lock (_lock)
                    {
                        if (!IsInitialized)
                        {
                            _value = _valueFactory(_state);
                            _valueFactory = null;
                            IsInitialized = true;
                        }
                    }
                }
                else
                {
                    if (!IsInitialized)
                    {
                        _value = _valueFactory(_state);
                        _valueFactory = null;
                        IsInitialized = true;
                    }
                }

                return _value;
            }
        }

        /// <inheritdoc/>
        [MemberNotNullWhen(false, nameof(_valueFactory))]
        [MemberNotNullWhen(false, nameof(_state))]
        [MemberNotNullWhen(true, nameof(_value))]
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Initializes a new instance of <see cref="SlimLazy{T, TState}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The Value.
        /// </param>
        public SlimLazy(T value)
        {
            _value = value;
            IsInitialized = true;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="SlimLazy{T, TState}"/> class with the provided factory delegate and state.
        ///     Optional you can indicate if the initialization should be thread-safe.
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <param name="threadSafe">
        ///     Indicates if the initialization should be thread-safe.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="valueFactory"/> or <paramref name="state"/> are <see langword="null"/>.
        /// </exception>
        public SlimLazy(Func<TState, T> valueFactory, TState state, bool threadSafe = false)
        {
            _valueFactory = Ensure.NotNull(valueFactory);
            _state = Ensure.NotNull(state);

            if (threadSafe)
                _lock = new object();
        }
    }

    /// <summary>
    ///     Provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public interface ILazy<out T>
    {
        /// <summary>
        ///     Gets the value.
        /// </summary>
        T Value { get; }

        /// <summary>
        ///     Indicates if the value is initialized.
        /// </summary>
        bool IsInitialized { get; }
    }
}

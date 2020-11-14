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
    public class SlimLazy<T> : ILazy<T>
    {
        /// <summary>
        ///     The value.
        /// </summary>
        protected T? _value;

        /// <summary>
        ///     The factory delegate.
        /// </summary>
        protected Func<T>? _valueFactory;

        /// <inheritdoc/>
        public virtual T Value
        {
            get
            {
                if (!IsInitialized)
                {
                    _value = _valueFactory();
                    _valueFactory = null;
                    IsInitialized = true;
                }

                return _value;
            }
        }

        /// <inheritdoc/>
        [MemberNotNullWhen(false, nameof(_valueFactory))]
        [MemberNotNullWhen(true, nameof(_value))]
        public virtual bool IsInitialized { get; protected set; }

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
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="valueFactory"/> is <see langword="null"/>.
        /// </exception>
        public SlimLazy(Func<T> valueFactory)
        {
            _valueFactory = Ensure.NotNull(valueFactory);
        }
    }

    /// <summary>
    ///     A thread-safe lightweight implementation of <see cref="Lazy{T}"/>, which also provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public sealed class ConcurrentLazy<T> : SlimLazy<T>
    {
        private readonly object _lock = new();

        /// <inheritdoc/>
        public override T Value
        {
            get
            {
                lock (_lock)
                    return base.Value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrentLazy{T}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        public ConcurrentLazy(T value) : base(value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrentLazy{T}"/> class with the provided factory delegate. 
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="valueFactory"/> is <see langword="null"/>.
        /// </exception>
        public ConcurrentLazy(Func<T> valueFactory) : base(valueFactory)
        {
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
    public class SlimLazy<T, TState> : ILazy<T> where TState : notnull
    {
        /// <summary>
        ///     The value.
        /// </summary>
        protected T? _value;

        /// <summary>
        ///     The value factory delegate.
        /// </summary>
        protected Func<TState, T>? _valueFactory;

        /// <summary>
        ///     The factory delegate arguments.
        /// </summary>
        protected readonly TState? _state;

        /// <inheritdoc/>
        public virtual T Value
        {
            get
            {
                if (!IsInitialized)
                {
                    _value = _valueFactory(_state);
                    _valueFactory = null;
                    IsInitialized = true;
                }

                return _value;
            }
        }

        /// <inheritdoc/>
        [MemberNotNullWhen(false, nameof(_valueFactory))]
        [MemberNotNullWhen(false, nameof(_state))]
        [MemberNotNullWhen(true, nameof(_value))]
        public virtual bool IsInitialized { get; protected set; }

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
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="valueFactory"/> or <paramref name="state"/> are <see langword="null"/>.
        /// </exception>
        public SlimLazy(Func<TState, T> valueFactory, TState state)
        {
            _valueFactory = Ensure.NotNull(valueFactory);
            _state = Ensure.NotNull(state);
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
    public class ConcurrentSlimLazy<T, TState> : SlimLazy<T, TState> where TState : notnull
    {
        private readonly object _lock = new();

        /// <inheritdoc/>
        public override T Value
        {
            get
            {
                lock (_lock)
                    return base.Value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="ConcurrentSlimLazy{T, TState}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The Value.
        /// </param>
        public ConcurrentSlimLazy(T value) : base(value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="ConcurrentSlimLazy{T, TState}"/> class with the provided factory delegate and state.
        /// </summary>
        /// <param name="valueFactory">
        ///     The value factory delegate.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="valueFactory"/> or <paramref name="state"/> are <see langword="null"/>.
        /// </exception>
        public ConcurrentSlimLazy(Func<TState, T> valueFactory, TState state) : base(valueFactory, state)
        {
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

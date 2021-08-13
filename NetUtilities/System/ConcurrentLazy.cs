namespace System
{
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

        /// <inheritdoc/>
        public override bool IsValueCreated
        {
            get
            {
                lock (_lock)
                    return base.IsValueCreated;
            }
            protected set
            {
                lock (_lock)
                    base.IsValueCreated = value;
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
    public sealed class ConcurrentLazy<T, TState> : SlimLazy<T, TState> where TState : notnull
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

        /// <inheritdoc/>
        public override bool IsValueCreated
        {
            get
            {
                lock (_lock)
                    return base.IsValueCreated;
            }
            protected set
            {
                lock (_lock)
                    base.IsValueCreated = value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="ConcurrentSlimLazy{T, TState}"/> class with the provided value.
        /// </summary>
        /// <param name="value">
        ///     The Value.
        /// </param>
        public ConcurrentLazy(T value) : base(value)
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
        public ConcurrentLazy(Func<TState, T> valueFactory, TState state) : base(valueFactory, state)
        {
        }
    }
}

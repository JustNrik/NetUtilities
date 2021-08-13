using NetUtilities;

namespace System
{
    /// <summary>
    ///     Represents a 3-states nullable value. 
    /// </summary>
    /// <remarks>
    ///     These 3 states are useful if you want to distinguish whether
    ///     <see langword="null"/> is intentional.
    /// </remarks>
    /// <typeparam name="T">
    ///     The underlying type of the <see cref="Optional{T}"/>.
    /// </typeparam>
    public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<T>
    {
        private static readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        private readonly T _value;
        private readonly bool _hasValue;

        /// <summary>
        ///     Returns a empty instance of <see cref="Optional{T}"/>
        /// </summary>
        public static Optional<T> Empty
            => default;

        /// <summary>
        ///     Gets the value of this <see cref="Optional{T}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when this optional doesn't have a value.
        /// </exception>
        public T Value
            => _hasValue
            ? _value
            : throw new InvalidOperationException("The optional has no value.");

        /// <summary>
        ///     Indicates if this <see cref="Optional{T}"/> has a value.
        /// </summary>
        public bool HasValue
            => _hasValue;

        /// <summary>
        ///     Initializes a new instance of <see cref="Optional{T}"/> <see langword="struct"/>
        ///     with the provided <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => obj switch
            {
                Optional<T> optional => Equals(optional),
                T value => Equals(value),
                IEquatable<Optional<T>> optionalEquatable => optionalEquatable.Equals(this),
                IEquatable<T> equatable => _hasValue && equatable.Equals(_value),
                _ => false
            };

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (typeof(T).IsValueType)
                return EqualityComparer<T>.Default.GetHashCode(_value!);

            return _value is null
                ? 0
                : _comparer.GetHashCode(_value);
        }

        /// <inheritdoc/>
        public override string? ToString()
            => _hasValue
            ? _value?.ToString() // intentional null return
            : string.Empty;

        /// <inheritdoc/>
        public bool Equals(Optional<T> optional)
        {
            if (!_hasValue)
                return !optional._hasValue;

            if (!optional._hasValue)
                return false;

            if (typeof(T).IsValueType) // JIT Optimizes this for value types
                return EqualityComparer<T>.Default.Equals(_value, optional._value);

            return _comparer.Equals(_value, optional._value);
        }

        /// <inheritdoc/>
        public bool Equals(T? value)
        {
            if (_hasValue)
                return false;

            if (typeof(T).IsValueType)
                return EqualityComparer<T>.Default.Equals(_value, value);

            return _comparer.Equals(_value, value);
        }

        public T GetValueOrDefault()
            => _value;

        public T OrDefault()
            => _value;

        public T OrElse(T other)
            => _hasValue ? _value : other;

        public T OrElseGet(Func<T> supplier)
            => _hasValue ? _value : Ensure.NotNull(supplier)();

        public T OrElseGet<TState>(Func<TState, T> supplier, TState state)
            => _hasValue ? _value : Ensure.NotNull(supplier)(state);

        public T OrElseThrow(Func<Exception> supplier)
            => _hasValue ? _value : throw Ensure.NotNull(supplier)();

        public T OrElseThrow<TState>(Func<TState, Exception> supplier, TState state)
            => _hasValue ? _value : throw Ensure.NotNull(supplier)(state);

        public Optional<T> Filter(Func<T, bool> predicate)
            => _hasValue && Ensure.NotNull(predicate)(_value) ? _value : Optional<T>.Empty;

        public Optional<T> Filter<TState>(Func<T, TState, bool> predicate, TState state)
            => _hasValue && Ensure.NotNull(predicate)(_value, state) ? _value : Optional<T>.Empty;

        public Optional<TResult> Map<TResult>(Func<T, TResult> func) where TResult : IEquatable<TResult>
            => _hasValue ? Ensure.NotNull(func)(_value) : Optional<TResult>.Empty;

        public Optional<TResult> Map<TResult, TState>(Func<T, TState, TResult> func, TState state) where TResult : IEquatable<TResult>
            => _hasValue ? Ensure.NotNull(func)(_value, state) : Optional<TResult>.Empty;

        public void IfPresent(Action<T> action)
        {
            if (_hasValue)
                Ensure.NotNull(action)(_value);
        }

        public void IfPresent<TState>(Action<T, TState> action, TState state)
        {
            if (_hasValue)
                Ensure.NotNull(action)(_value, state);
        }

        public static bool operator ==(Optional<T> left, Optional<T> right)
            => left.Equals(right);

        public static bool operator !=(Optional<T> left, Optional<T> right)
            => !left.Equals(right);

        public static implicit operator Optional<T>(T value)
            => new(value);

        public static explicit operator T(Optional<T> optional)
            => optional.Value; // intentional, it will throw if it has no value
    }

    public static class Optional
    {
        public static Optional<T> Of<T>(T value)
            => new(value);
    }
}

using System.Collections.Generic;
using NetUtilities;

namespace System
{
    public readonly struct Optional<T> : IEquatable<Optional<T>>, IEquatable<T>
    {
        private static readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        private readonly T _value;
        private readonly bool _hasValue;

        public static Optional<T> Empty => default;

        public T Value => _hasValue ? _value : throw new InvalidOperationException("FFS");

        public bool HasValue => _hasValue;

        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public override bool Equals(object? obj)
            => obj is Optional<T> optional && Equals(optional)
            || obj is T value && Equals(value);

        public override int GetHashCode()
            => _value?.GetHashCode() ?? 0;

        public override string? ToString()
            => _value?.ToString();

        public bool Equals(Optional<T> optional)
        {
            if (!_hasValue)
                return !optional._hasValue;

            return _comparer.Equals(_value, optional._value);
        }

        public bool Equals(T? value)
        {
            if (!_hasValue) // explicitly disallowing struct == null being true.
                return default(T) is not null && _comparer.Equals(value, default(T));

            return _comparer.Equals(_value, value);
        }

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
        {
            if (!left._hasValue)
                return !right._hasValue;

            return right._hasValue && left.Equals(right);
        }

        public static bool operator !=(Optional<T> left, Optional<T> right)
        {
            if (!left._hasValue)
                return right._hasValue;

            return !right._hasValue || !left.Equals(right);
        }

        public static implicit operator Optional<T>(T value)
            => new Optional<T>(value);
    }

    public static class Optional
    {
        public static Optional<T> Of<T>(T value)
            => new Optional<T>(value);
    }
}

using System.Globalization;
using System.Runtime.InteropServices;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     Represents a 24-bit unsigned integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct UInt24 : 
        IEquatable<UInt24>, IComparable<UInt24>, 
        IEquatable<UInt24?>, IComparable<UInt24?>, 
        IConvertible, IComparable, IFormattable
    {
        internal readonly uint _value;

        /// <summary>
        ///     Represents the largest possible value of <see cref="UInt24"/>. This field is constant.
        /// </summary>
        public const uint MaxValue = 0xFFFFFF;

        /// <summary>
        ///     Represents the smallest possible value of <see cref="UInt24"/>. This field is constant.
        /// </summary>
        public const uint MinValue = 0;

        /// <summary>
        ///     Creates a new instance of <see cref="UInt24"/> <see langword="struct"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="value"/> is higher than <see cref="MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///     The value.
        /// </param>
        public UInt24(Int24 value) : this((uint)value._value)
        {
        }

        /// <summary>
        ///     Creates a new instance of <see cref="UInt24"/> <see langword="struct"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="value"/> is higher than <see cref="MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///     The value.
        /// </param>
        public UInt24(int value) : this((uint)value)
        {
        }

        /// <summary>
        ///     Creates a new instance of <see cref="UInt24"/> <see langword="struct"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="value"/> is higher than <see cref="MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///     The value.
        /// </param>
        public UInt24(uint value)
        {
            Ensure.NotOutOfRange(value <= MaxValue, value);
            _value = value;
        }

        #region overrided from System.Object
        /// <inheritdoc cref="uint.ToString()"/>
        public override string ToString()
            => _value.ToString();

        /// <inheritdoc cref="uint.GetHashCode()"/>
        public override int GetHashCode()
            => (int)_value;

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => _value.Equals(obj);
        #endregion
        #region interfaces implementation
        /// <inheritdoc/>
        public bool Equals(UInt24 other)
            => _value == other._value;

        /// <inheritdoc/>
        public bool Equals(UInt24? other)
            => _value == other?._value;

        /// <inheritdoc/>
        public int CompareTo(UInt24 other)
            => _value.CompareTo(other._value);

        public int CompareTo(UInt24? other)
            => other is null ? 1 : _value.CompareTo(other.Value._value);

        int IComparable.CompareTo(object obj)
            => obj is UInt24 uInt24 ? _value.CompareTo(uInt24._value) : throw new ArgumentException(nameof(obj));

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
            => _value.ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode()
            => TypeCode.UInt32;

        bool IConvertible.ToBoolean(IFormatProvider? provider)
            => Convert.ToBoolean(_value, provider);

        byte IConvertible.ToByte(IFormatProvider? provider)
            => Convert.ToByte(_value, provider);

        char IConvertible.ToChar(IFormatProvider? provider)
            => Convert.ToChar(_value, provider);

        DateTime IConvertible.ToDateTime(IFormatProvider? provider)
            => Convert.ToDateTime(_value, provider);

        decimal IConvertible.ToDecimal(IFormatProvider? provider)
            => Convert.ToDecimal(_value, provider);

        double IConvertible.ToDouble(IFormatProvider? provider)
            => Convert.ToDouble(_value, provider);

        short IConvertible.ToInt16(IFormatProvider? provider)
            => Convert.ToInt16(_value, provider);

        int IConvertible.ToInt32(IFormatProvider? provider)
            => Convert.ToInt32(_value, provider);

        long IConvertible.ToInt64(IFormatProvider? provider)
            => Convert.ToInt64(_value, provider);

        sbyte IConvertible.ToSByte(IFormatProvider? provider)
            => Convert.ToSByte(_value, provider);

        float IConvertible.ToSingle(IFormatProvider? provider)
            => Convert.ToSingle(_value, provider);

        string IConvertible.ToString(IFormatProvider? provider)
            => Convert.ToString(_value, provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
            => throw new NotSupportedException();

        ushort IConvertible.ToUInt16(IFormatProvider? provider)
            => Convert.ToUInt16(_value, provider);

        uint IConvertible.ToUInt32(IFormatProvider? provider)
            => Convert.ToUInt32(_value, provider);

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
            => Convert.ToUInt64(_value, provider);
        #endregion
        #region operators
        /// <summary>
        ///     Indicates of both instances represent the same value.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///    <see langword="true"/> if both instances represent the same value; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(UInt24 left, UInt24 right)
            => left._value == right._value;

        /// <summary>
        ///     Indicates of both instances represent different values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///    <see langword="true"/> if both instances represent different values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(UInt24 left, UInt24 right)
            => left._value != right._value;

        /// <summary>
        ///     Indicates of both instances represent the same value.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///    <see langword="true"/> if both instances represent the same value; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(UInt24 left, Int24 right)
            => left._value == right._value;

        /// <summary>
        ///     Indicates of both instances represent different values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///    <see langword="true"/> if both instances represent different values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(UInt24 left, Int24 right)
            => left._value == right._value;

        /// <summary>
        ///     Computes the bitwise logical AND of the integral value of the instances.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///     The bitwise logical AND of the integral value of the instances.
        /// </returns>
        public static UInt24 operator &(UInt24 left, UInt24 right)
            => new UInt24(left._value & right._value);

        /// <summary>
        ///     Computes the bitwise logical OR of the integral value of the instances.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///     The bitwise logical OR of the integral value of the instances.
        /// </returns>
        public static UInt24 operator |(UInt24 left, UInt24 right)
            => new UInt24(left._value | right._value);

        /// <summary>
        ///     Computes the bitwise logical XOR of the integral value of the instances.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///     The bitwise logical XOR of the integral value of the instances.
        /// </returns>
        public static UInt24 operator ^(UInt24 left, UInt24 right)
            => new UInt24((left._value ^ right._value) & MaxValue);

        /// <summary>
        ///     Computes the bitwise complement of the integral value of the instance.
        /// </summary>
        /// <param name="uInt24">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The bitwise complement of the integral value of the instance.
        /// </returns>
        public static UInt24 operator ~(UInt24 uInt24)
            => new UInt24(~uInt24._value & MaxValue);

        /// <summary>
        ///     Computes the sum of both values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///     The sum of both values.
        /// </returns>
        public static UInt24 operator +(UInt24 left, UInt24 right)
            => new UInt24((left._value + right._value) % (MaxValue + 1));

        /// <summary>
        ///     Computes the substraction of both values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <returns>
        ///     The substraction of both values.
        /// </returns>
        public static UInt24 operator -(UInt24 left, UInt24 right)
            => left._value > right._value 
            ? new UInt24(left._value - right._value)
            : new UInt24(MaxValue - right._value - left._value + 1);

        /// <summary>
        ///     Computes the divition of both values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <exception cref="DivideByZeroException">
        ///     Throw when <paramref name="right"/> is zero.
        /// </exception>
        /// <returns>
        ///     The divition of both values.
        /// </returns>
        public static UInt24 operator /(UInt24 left, UInt24 right)
            => new UInt24(left._value / right._value);

        /// <summary>
        ///     Computes the multiplication of both values.
        /// </summary>
        /// <param name="left">
        ///     The left value.
        /// </param>
        /// <param name="right">
        ///     The right value.
        /// </param>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be too large.
        /// </exception>
        /// <returns>
        ///     The multiplication of both values.
        /// </returns>
        public static UInt24 operator *(UInt24 left, UInt24 right)
            => new UInt24(checked(left._value * right._value));
        #endregion
        #region casts
        // widening casts
        public static implicit operator int(UInt24 uInt24)
            => (int)uInt24._value;
        public static implicit operator uint(UInt24 uInt24)
            => uInt24._value;
        public static implicit operator long(UInt24 uInt24)
            => uInt24._value;
        // narrowing casts from UInt24
        public static explicit operator byte(UInt24 uInt24)
            => (byte)uInt24._value;
        public static explicit operator sbyte(UInt24 uInt24)
            => (sbyte)uInt24._value;
        public static explicit operator short(UInt24 uInt24)
            => (short)uInt24._value;
        public static explicit operator ushort(UInt24 uInt24)
            => (ushort)uInt24._value;
        public static explicit operator Int24(UInt24 uInt24)
            => new Int24(uInt24);
        // narrowing casts to UInt24
        public static explicit operator UInt24(Int24 int24)
            => new UInt24((uint)int24);
        public static explicit operator UInt24(int int32)
            => new UInt24((uint)int32);
        public static explicit operator UInt24(uint uInt32)
            => new UInt24(uInt32);
        public static explicit operator UInt24(long int64)
            => new UInt24((uint)int64);
        public static explicit operator UInt24(ulong uInt64)
            => new UInt24((uint)uInt64);
        #endregion
        #region static methods
        public static UInt24 Parse(string input)
            => Parse(input, NumberStyles.Integer, null);

        public static UInt24 Parse(string input, NumberStyles style)
            => Parse(input, style, null);

        public static UInt24 Parse(string input, NumberStyles style, IFormatProvider? provider)
        {
            Ensure.NotNull(input);

            if (uint.TryParse(input, style, provider, out var parsed))
            {
                if (parsed > MaxValue)
                    throw new OverflowException("The value represented by the string is outside of the allowed ranged.");

                return new UInt24(parsed);
            }

            throw new FormatException("The string is not in a valid format");
        }

        public static bool TryParse(string? input, out UInt24 result)
            => TryParse(input, NumberStyles.Integer, null, out result);

        public static bool TryParse(string? input, NumberStyles style, out UInt24 result)
            => TryParse(input, style, null, out result);

        public static bool TryParse(string? input, NumberStyles style, IFormatProvider? provider, out UInt24 result)
        {
            if (uint.TryParse(input, style, provider, out var parsed) && parsed <= MaxValue)
            {
                result = new UInt24(parsed);
                return true;
            }

            result = default;
            return false;
        }
        #endregion
    }
}
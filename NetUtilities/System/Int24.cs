using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     Represents a 24-bit signed integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3 /* Size 3 will cut off LSB. Depending on machine endianness, this might be very much undesirable. */, Pack = 1)]
    public readonly struct Int24 : IEquatable<Int24>, IComparable<Int24>, IConvertible, IComparable, IFormattable
    {
        internal readonly int _value;
        private const int Mask = 0xFFFFFF;

        /// <summary>
        ///     Represents the largest possible value of an <see cref="Int24"/>. This field is constant.
        /// </summary>
        public const int MaxValue = 8388607;

        /// <summary>
        ///     Represents the smallest possible value of <see cref="Int24"/>. This field is constant.
        /// </summary>
        public const int MinValue = -8388608;

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="UInt24"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(UInt24 value)
        {
            if (value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value._value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="uint"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(uint value)
        {
            if (value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="int"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(int value)
        {
            if ((uint)value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="ulong"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(ulong value)
        {
            if (value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="byte"/>.
        /// </summary>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(byte value)
        {
            _value = value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="IntPtr"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(nint value)
        {
            if ((nuint)value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="UIntPtr"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(nuint value)
        {
            if (value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="sbyte"/>.
        /// </summary>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(sbyte value)
        {
            _value = value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="short"/>.
        /// </summary>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(short value)
        {
            _value = value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="ushort"/>.
        /// </summary>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(ushort value)
        {
            _value = value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> to the value of the specified <see cref="long"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="value"/> is a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <param name="value">
        ///      The value to represent as a <see cref="Int24"/>.
        /// </param>
        public Int24(long value)
        {
            if ((ulong)value > MaxValue)
                throw new ArgumentOutOfRangeException($"The value must be between {MinValue} and {MaxValue}");

            _value = (int)value;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> from three bytes in the byte array.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="value"/> length is not 3.
        /// </exception>
        /// <param name="value">
        ///      The array to convert into a <see cref="Int24"/>.
        /// </param>
        public Int24(byte[] bytes) : this(new ReadOnlySpan<byte>(bytes))
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> from three bytes at a specific position in the byte array.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="startIndex"/> is outside the bounds of the array. 
        ///     -or- 
        ///     the range from the <paramref name="startIndex"/> to the span length is not 3.
        /// </exception>
        /// <param name="value">
        ///      The array to convert into a <see cref="Int24"/>.
        /// </param>
        public Int24(byte[] bytes, int startIndex) : this(new ReadOnlySpan<byte>(bytes, startIndex, 3))
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Int24"/> from three bytes in the span.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="value"/> length is not 3.
        /// </exception>
        /// <param name="value">
        ///      The span to convert into a <see cref="Int24"/>.
        /// </param>
        public Int24(ReadOnlySpan<byte> span)
        {
            if (span.Length != 3)
                throw new ArgumentException("The input bytes must have a length of 3.");

            this = Unsafe.ReadUnaligned<Int24>(ref MemoryMarshal.GetReference(span));
        }

        #region instance methods
        /// <summary>
        ///     Returns this instance's value as a byte array.
        /// </summary>
        /// <returns>
        ///     This instance's value as a byte array.
        /// </returns>
        public byte[] GetBytes()
        {
            var bytes = new byte[3];

            Unsafe.WriteUnaligned(ref bytes[0], this);
            return bytes;
        }
        #endregion

        #region overriden from System.Object
        public override string ToString()
            => _value.ToString();

        public override int GetHashCode()
            => _value;

        public override bool Equals(object obj)
            => obj is Int24 i24 ? Equals(i24) : _value.Equals(obj);
        #endregion

        #region interface implementations
        public bool Equals(Int24 other)
            => _value == other._value;

        public int CompareTo(Int24 other)
            => _value.CompareTo(other._value);

        int IComparable.CompareTo(object obj)
            => _value.CompareTo(obj);

        public string ToString(string format, IFormatProvider formatProvider)
            => _value.ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode()
            => TypeCode.Int32;

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => Convert.ToBoolean(_value, provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => Convert.ToByte(_value, provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => Convert.ToChar(_value, provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => Convert.ToDateTime(_value, provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => Convert.ToDecimal(_value, provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => Convert.ToDouble(_value, provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => Convert.ToInt16(_value, provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => Convert.ToInt32(_value, provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => Convert.ToInt64(_value, provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => Convert.ToSByte(_value, provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => Convert.ToSingle(_value, provider);

        string IConvertible.ToString(IFormatProvider provider)
            => Convert.ToString(_value, provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => ((IConvertible)_value).ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => Convert.ToUInt16(_value, provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => Convert.ToUInt32(_value, provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => Convert.ToUInt64(_value, provider);
        #endregion

        #region operators
        // equality
        public static bool operator ==(Int24 left, Int24 right)
            => left._value == right._value;
        public static bool operator !=(Int24 left, Int24 right)
            => left._value != right._value;
        public static bool operator ==(Int24 left, int right)
            => left._value == right;
        public static bool operator !=(Int24 left, int right)
            => left._value != right;
        public static bool operator ==(int left, Int24 right)
            => left == right._value;
        public static bool operator !=(int left, Int24 right)
            => left != right._value;
        public static bool operator ==(Int24 left, UInt24 right)
            => left._value == right._value;
        public static bool operator !=(Int24 left, UInt24 right)
            => left._value != right._value;

        // comparison
        public static bool operator >(Int24 left, Int24 right)
            => left._value > right._value;
        public static bool operator >(Int24 left, int right)
            => left._value > right;
        public static bool operator >(int left, Int24 right)
            => left > right._value;
        public static bool operator <(Int24 left, Int24 right)
            => left._value < right._value;
        public static bool operator <(Int24 left, int right)
            => left._value < right;
        public static bool operator <(int left, Int24 right)
            => left < right._value;
        public static bool operator >=(Int24 left, Int24 right)
            => left._value >= right._value;
        public static bool operator >=(Int24 left, int right)
            => left._value >= right;
        public static bool operator >=(int left, Int24 right)
            => left >= right._value;
        public static bool operator <=(Int24 left, Int24 right)
            => left._value <= right._value;
        public static bool operator <=(Int24 left, int right)
            => left._value <= right;
        public static bool operator <=(int left, Int24 right)
            => left <= right._value;

        // bitwise
        public static Int24 operator &(Int24 left, Int24 right)
            => new Int24(left._value & right._value);
        public static Int24 operator &(Int24 left, int right)
            => new Int24(left._value & right);
        public static Int24 operator &(int left, Int24 right)
            => new Int24(left & right._value);
        public static Int24 operator |(Int24 left, Int24 right)
            => new Int24(left._value | right._value);
        public static Int24 operator |(Int24 left, int right)
            => new Int24(left._value | right);
        public static Int24 operator |(int left, Int24 right)
            => new Int24(left | right._value);
        public static Int24 operator ^(Int24 left, Int24 right)
            => new Int24(left._value ^ right._value);
        public static Int24 operator ^(Int24 left, int right)
            => new Int24((left._value ^ right) & Mask);
        public static Int24 operator ^(int left, Int24 right)
            => new Int24((left ^ right._value) & Mask);
        public static Int24 operator <<(Int24 left, int right)
            => new Int24((left._value << right) & Mask);
        public static Int24 operator >>(Int24 left, int right)
            => new Int24(left._value >> right);
        public static Int24 operator ~(Int24 int24)
            => new Int24(~int24._value & MaxValue);

        // unary arithmetic
        public static Int24 operator ++(Int24 int24)
            => new Int24(int24._value + 1);
        public static Int24 operator --(Int24 int24)
            => new Int24(int24._value - 1);
        public static Int24 operator +(Int24 int24)
            => int24;
        public static Int24 operator -(Int24 int24)
            => new Int24(-int24._value);

        // binary arithmetic
        public static Int24 operator +(Int24 left, Int24 right)
            => new Int24(left._value + right._value);
        public static Int24 operator +(Int24 left, int right)
            => new Int24(left._value + right);
        public static Int24 operator +(int left, Int24 right)
            => new Int24(left + right._value);
        public static Int24 operator -(Int24 left, Int24 right)
            => new Int24(left._value - right._value);
        public static Int24 operator -(Int24 left, int right)
            => new Int24(left._value - right);
        public static Int24 operator -(int left, Int24 right)
            => new Int24(left - right._value);
        public static Int24 operator /(Int24 left, Int24 right)
            => new Int24(left._value / right._value);
        public static Int24 operator /(Int24 left, int right)
            => new Int24(left._value / right);
        public static Int24 operator /(int left, Int24 right)
            => new Int24(left / right._value);
        public static Int24 operator *(Int24 left, Int24 right)
            => new Int24(left._value * right._value);
        public static Int24 operator *(Int24 left, int right)
            => new Int24(left._value * right);
        public static Int24 operator *(int left, Int24 right)
            => new Int24(left * right._value);
        public static Int24 operator %(Int24 left, Int24 right)
            => new Int24(left._value % right._value);
        public static Int24 operator %(int left, Int24 right)
            => new Int24(left % right._value);
        public static Int24 operator %(Int24 left, int right)
            => new Int24(left._value % right);
        #endregion

        #region casts
        // widening casts
        public static implicit operator int(Int24 int24)
            => int24._value;
        public static implicit operator long(Int24 int24)
            => int24._value;
        public static implicit operator nint(Int24 int24)
            => int24._value;
        // narrowing casts from Int24
        public static explicit operator byte(Int24 int24)
            => (byte)int24._value;
        public static explicit operator sbyte(Int24 int24)
            => (sbyte)int24._value;
        public static explicit operator short(Int24 int24)
            => (short)int24._value;
        public static explicit operator ushort(Int24 int24)
            => (ushort)int24._value;
        public static explicit operator uint(Int24 int24)
            => (uint)int24._value;
        public static explicit operator ulong(Int24 int24)
            => (ulong)int24._value;
        public static explicit operator nuint(Int24 int24)
            => (nuint)int24._value;
        // narrowing casts to Int24
        public static explicit operator Int24(UInt24 uInt24)
            => new Int24(uInt24);
        public static explicit operator Int24(int int32)
            => new Int24(int32);
        public static explicit operator Int24(uint uInt32)
            => new Int24((int)uInt32);
        public static explicit operator Int24(long int64)
            => new Int24((int)int64);
        public static explicit operator Int24(ulong uInt64)
            => new Int24((int)uInt64);
        public static explicit operator Int24(nint intPtr)
            => new Int24(intPtr);
        public static explicit operator Int24(nuint uIntPtr)
            => new Int24(uIntPtr);
        #endregion

        #region Static methods
        /// <summary>
        ///     Converts the string representation of a number to its 24-bit signed integer equivalent.
        /// </summary>
        /// <param name="input">
        ///     A string containing a number to convert.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when <paramref name="input"/> is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when <paramref name="input"/> represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        /// </exception>
        /// <returns>
        ///     A 24-bit signed integer equivalent to the number contained in <paramref name="input"/>.
        /// </returns>
        public static Int24 Parse(string input)
            => Parse(input, NumberStyles.Integer, null);

        /// <summary>
        ///     Converts the string representation of a number in a specified style to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="input">
        ///     A string containing a number to convert.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="input"/>. 
        ///     A typical value to specify is <see cref="NumberStyles.Integer"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="style"/> is not a <see cref="NumberStyles.Integer"/> value.
        ///     -or- 
        ///     <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when <paramref name="input"/> is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when <paramref name="input"/> represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        ///     -or- 
        ///     <paramref name="input"/> includes non-zero, fractional digits.
        /// </exception>
        /// <returns>
        ///     A 24-bit signed integer equivalent to the number contained in <paramref name="input"/>.
        /// </returns>
        public static Int24 Parse(string input, NumberStyles style)
            => Parse(input, style, null);

        /// <summary>
        ///     Converts the string representation of a number in a specified style and culture-specific format to its 24-bit signed integer equivalent.
        /// </summary>
        /// <param name="input">
        ///     A string containing a number to convert.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="input"/>. 
        ///     A typical value to specify is <see cref="NumberStyles.Integer"/>.
        /// </param>
        /// <param name="provider">
        ///     An object that supplies culture-specific information about the format of <paramref name="input"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="style"/> is not a <see cref="NumberStyles.Integer"/> value.
        ///     -or- 
        ///     <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when <paramref name="input"/> is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when <paramref name="input"/> represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        ///     -or- 
        ///     <paramref name="input"/> includes non-zero, fractional digits.
        /// </exception>
        /// <returns>
        ///     A 24-bit signed integer equivalent to the number contained in <paramref name="input"/>.
        /// </returns>
        public static Int24 Parse(string input, NumberStyles style, IFormatProvider? provider)
            => Parse(Ensure.NotNull(input).AsSpan(), style, provider);

        /// <summary>
        ///     Converts the span representation of a number in a specified style and culture-specific format to its 24-bit signed integer equivalent.
        /// </summary>
        /// <param name="span">
        ///     A span containing the characters representing the number to convert.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="span"/>. 
        ///     A typical value to specify is <see cref="NumberStyles.Integer"/>.
        /// </param>
        /// <param name="provider">
        ///     An object that supplies culture-specific information about the format of <paramref name="span"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="style"/> is not a <see cref="NumberStyles.Integer"/> value.
        ///     -or- 
        ///     <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when <paramref name="span"/> is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when <paramref name="span"/> represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>.
        ///     -or- 
        ///     <paramref name="span"/> includes non-zero, fractional digits.
        /// </exception>
        /// <returns>
        ///     A 24-bit signed integer equivalent to the number contained in <paramref name="span"/>.
        /// </returns>
        public static Int24 Parse(ReadOnlySpan<char> span, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
        {
            if (int.TryParse(span, style, provider, out var parsed))
            {
                if ((uint)parsed > MaxValue)
                    throw new OverflowException("The value represented by the string is outside of the allowed range.");

                return new Int24(parsed);
            }

            throw new FormatException("The string is not in a valid format");
        }

        /// <summary>
        ///     Converts the string representation of a number to its 24-bit signed integer equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        ///     A string containing a number to convert.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the 24-bit signed integer value equivalent of the number contained in <paramref name="input"/>, 
        ///     if the conversion succeeded, or zero if the conversion failed. 
        ///     The conversion fails if the <paramref name="input"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, is not of the correct format, 
        ///     or represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>. This parameter is passed uninitialized;
        ///     any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse([NotNullWhen(true)] string? input, out Int24 result)
            => TryParse(input, NumberStyles.Integer, null, out result);

        /// <summary>
        ///     Converts the span representation of a number to its 24-bit signed integer equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="span">
        ///     A span containing the characters that represent the number to convert.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the 24-bit signed integer value equivalent of the number contained in <paramref name="span"/>, 
        ///     if the conversion succeeded, or zero if the conversion failed. 
        ///     The conversion fails if the <paramref name="span"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, is not of the correct format, 
        ///     or represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>. This parameter is passed uninitialized;
        ///     any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="span"/> was converted successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> span, out Int24 result)
            => TryParse(span, NumberStyles.Integer, null, out result);

        /// <summary>
        ///     Converts the string representation of a number in a specified style and culture-specific format to its 24-bit signed integer equivalent. 
        ///     A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">
        ///     A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style"/>.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="input"/>. 
        ///     A typical value to specify is <see cref="NumberStyles.Integer"/>.
        /// </param>
        /// <param name="provider">
        ///     An object that supplies culture-specific information about the format of <paramref name="input"/>.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the 24-bit signed integer value equivalent of the number contained in <paramref name="input"/>, 
        ///     if the conversion succeeded, or zero if the conversion failed. 
        ///     The conversion fails if the <paramref name="input"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, is not of the correct format, 
        ///     or represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>. This parameter is passed uninitialized;
        ///     any value originally supplied in result will be overwritten.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="style"/> is not a <see cref="NumberStyles.Integer"/> value.
        ///     -or- 
        ///     <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.
        /// </exception>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="input"/> was converted successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse([NotNullWhen(true)] string? input, NumberStyles style, IFormatProvider? provider, out Int24 result)
            => TryParse((input ?? string.Empty).AsSpan(), style, provider, out result);

        /// <summary>
        ///     Converts the span representation of a number in a specified style and culture-specific format to its 24-bit signed integer equivalent. 
        ///     A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="span">
        ///     A span containing the characters that represent the number to convert. The span is interpreted using the style specified by <paramref name="style"/>.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="span"/>. 
        ///     A typical value to specify is <see cref="NumberStyles.Integer"/>.
        /// </param>
        /// <param name="provider">
        ///     An object that supplies culture-specific information about the format of <paramref name="span"/>.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the 24-bit signed integer value equivalent of the number contained in <paramref name="input"/>, 
        ///     if the conversion succeeded, or zero if the conversion failed. 
        ///     The conversion fails if the <paramref name="span"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>, is not of the correct format, 
        ///     or represents a number less than <see cref="Int24.MinValue"/> or greater than <see cref="Int24.MaxValue"/>. This parameter is passed uninitialized;
        ///     any value originally supplied in result will be overwritten.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="style"/> is not a <see cref="NumberStyles.Integer"/> value.
        ///     -or- 
        ///     <paramref name="style"/> is not a combination of <see cref="NumberStyles.AllowHexSpecifier"/> and <see cref="NumberStyles.HexNumber"/> values.
        /// </exception>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="span"/> was converted successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> span, NumberStyles style, IFormatProvider? provider, out Int24 result)
        {
            if (int.TryParse(span, style, provider, out var parsed) && (uint)parsed <= MaxValue)
            {
                result = new Int24(parsed);
                return true;
            }

            result = default;
            return false;
        }
        #endregion
    }
}

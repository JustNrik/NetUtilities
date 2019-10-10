using NetUtilities;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Represents a 24-bit unsigned integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct UInt24 : IEquatable<UInt24>, IComparable<UInt24>, IConvertible, IComparable, IFormattable
    {
        internal readonly uint _value;

        public const uint MaxValue = 0xFFFFFF;
        public const uint MinValue = 0;

        public UInt24(Int24 value) : this((uint)value._value)
        {
        }

        public UInt24(int value) : this((uint)value)
        {
        }

        public UInt24(uint value)
        {
            if (value > MaxValue)
                Throw.InvalidOperation("The value is outside the range of admited values.");

            _value = value;
        }

        #region overrided from System.Object
        public override string ToString()
            => _value.ToString();

        public override int GetHashCode()
            => unchecked((int)_value);

        public override bool Equals(object obj)
            => _value.Equals(obj);
        #endregion
        #region interfaces implementation
        public bool Equals(UInt24 other)
            => _value == other._value;

        public int CompareTo(UInt24 other)
            => _value.CompareTo(other._value);

        int IComparable.CompareTo(object obj)
            => obj is UInt24 uInt24 ? _value.CompareTo(uInt24._value) : throw new ArgumentException(nameof(obj));

        public string ToString(string format, IFormatProvider formatProvider)
            => _value.ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode()
            => TypeCode.UInt32;

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
            => throw new NotSupportedException();

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => Convert.ToUInt16(_value, provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => Convert.ToUInt32(_value, provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => Convert.ToUInt64(_value, provider);
        #endregion
        #region operators
        public static bool operator ==(UInt24 left, UInt24 right)
            => left._value == right._value;
        public static bool operator !=(UInt24 left, UInt24 right)
            => left._value != right._value;
        public static bool operator ==(UInt24 left, Int24 right)
            => left._value == right._value;
        public static bool operator !=(UInt24 left, Int24 right)
            => left._value == right._value;
        public static bool operator ==(UInt24 left, ulong right)
            => left._value == right;
        public static bool operator !=(UInt24 left, ulong right)
            => left._value != right;
        public static bool operator ==(ulong left, UInt24 right)
            => left == right._value;
        public static bool operator !=(ulong left, UInt24 right)
            => left != right._value;
        public static bool operator ==(UInt24 left, long right)
            => left._value == right;
        public static bool operator !=(UInt24 left, long right)
            => left._value != right;
        public static bool operator ==(long left, UInt24 right)
            => left == right._value;
        public static bool operator !=(long left, UInt24 right)
            => left != right._value;
        public static UInt24 operator &(UInt24 left, UInt24 right)
            => new UInt24(left._value & right._value);
        public static UInt24 operator |(UInt24 left, UInt24 right)
            => new UInt24(left._value | right._value);
        public static UInt24 operator ^(UInt24 left, UInt24 right)
            => new UInt24(left._value ^ right._value);
        public static UInt24 operator ~(UInt24 uInt24)
            => new UInt24(~uInt24._value & MaxValue);
        public static UInt24 operator +(UInt24 left, UInt24 right)
            => new UInt24(left._value + right._value);
        public static UInt24 operator -(UInt24 left, UInt24 right)
            => new UInt24(left._value - right._value);
        public static UInt24 operator /(UInt24 left, UInt24 right)
            => new UInt24(left._value / right._value);
        public static UInt24 operator *(UInt24 left, UInt24 right)
            => new UInt24(left._value * right._value);
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
        {
            if (input is null)
                Throw.NullArgument(nameof(input));

            if (uint.TryParse(input, out var parsed))
            {
                if (parsed > MaxValue)
                    Throw.Overflow("The value represented by the string is outside of the allowed ranged.");

                return new UInt24(parsed);
            }

            Throw.InvalidFormat("The string is not in a valid format");
            return default;
        }

        public static bool TryParse(string input, out UInt24 result)
        {
            if (int.TryParse(input, out var parsed) && parsed <= MaxValue)
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
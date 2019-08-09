using System.Runtime.InteropServices;
#nullable enable
namespace System
{
    /// <summary>
    /// Represents a 24-bit signed integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct Int24 : IEquatable<Int24>, IComparable<Int24>, IConvertible, IComparable, IFormattable
    {
        internal readonly int _value;

        public const int MaxValue = 0x7FFFFF;
        public const int MinValue = unchecked((int)0xFFFFFFFFFF800000);

        public Int24(UInt24 value) : this(unchecked((int)value._value))
        {
        }

        public Int24(uint value) : this(unchecked((int)value))
        {
        }

        public Int24(int value)
        {
            EnsureRange(value);
            _value = value;
        }

        public Int24(byte[] bytes) : this(BitConverter.ToInt32(bytes))
        {
        }

        private static void EnsureRange(int value)
        {
            if (value < MinValue || value > MaxValue)
                throw new OverflowException(nameof(value));
        }

        #region overrided from System.Object
        public override string ToString()
            => _value.ToString();

        public override int GetHashCode()
            => _value;

        public override bool Equals(object obj)
            => _value.Equals(obj);
        #endregion
        #region interfaces implementation
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
            => throw new NotSupportedException();

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => Convert.ToUInt16(_value, provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => Convert.ToUInt32(_value, provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => Convert.ToUInt64(_value, provider);
        #endregion
        #region operators
        public static bool operator ==(Int24 left, Int24 right)
            => left._value == right._value;
        public static bool operator !=(Int24 left, Int24 right)
            => left._value != right._value;
        public static bool operator ==(Int24 left, UInt24 right)
            => left._value == right._value;
        public static bool operator !=(Int24 left, UInt24 right)
            => left._value != right._value;
        public static bool operator ==(Int24 left, int right)
            => left._value == right;
        public static bool operator !=(Int24 left, int right)
            => left._value != right;
        public static bool operator ==(int left, Int24 right)
            => left == right._value;
        public static bool operator !=(int left, Int24 right)
            => left != right._value;
        public static Int24 operator &(Int24 left, Int24 right)
            => new Int24(left._value & right._value);
        public static Int24 operator |(Int24 left, Int24 right)
            => new Int24(left._value | right._value);
        public static Int24 operator ^(Int24 left, Int24 right)
            => new Int24(left._value ^ right._value);
        public static Int24 operator ~(Int24 int24)
            => new Int24(~int24._value & MaxValue);
        public static Int24 operator +(Int24 left, Int24 right)
            => new Int24(left._value + right._value);
        public static Int24 operator -(Int24 left, Int24 right)
            => new Int24(left._value - right._value);
        public static Int24 operator /(Int24 left, Int24 right)
            => new Int24(left._value / right._value);
        public static Int24 operator *(Int24 left, Int24 right)
            => new Int24(left._value * right._value);
        #endregion
        #region casts
        // widening casts
        public static implicit operator int(Int24 int24)
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
        public static explicit operator UInt24(Int24 int24)
            => new UInt24(int24);
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
        #endregion
    }
}

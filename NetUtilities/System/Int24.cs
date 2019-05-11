using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Represents a 24-bit signed integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct Int24 : IEquatable<Int24>, IComparable<Int24>, IConvertible, IComparable, IFormattable
    {
        private readonly byte _byte;
        private readonly short _short;

        public const int MaxValue = 0x7FFFFF;
        public const int MinValue = unchecked((int)0xFFFFFFFFFF800000);

        public Int24(byte value)
        {
            _byte = value;
            _short = 0;
        }

        public Int24(int value)
        {
            EnsureRange(value);

            _byte = (byte)(value & 0xFF);
            _short = (short)(value >> 8);
        }

        private static void EnsureRange(int value)
        {
            if (value < MinValue || value > MaxValue)
                throw new OverflowException(nameof(value));
        }

        #region overrided from System.Object
        public override string ToString()
            => ((int)this).ToString();

        public override int GetHashCode()
            => this;

        public override bool Equals(object obj)
            => obj is Int24 i24 ? Equals(i24) : false;
        #endregion
        #region interfaces implementation
        public bool Equals(Int24 other)
            => _byte == other._byte && _short == other._short;

        public int CompareTo(Int24 other)
            => ((int)this).CompareTo(other);

        int IComparable.CompareTo(object obj)
            => obj is Int24 i24 ? CompareTo(i24) : throw new ArgumentException(nameof(obj));

        public string ToString(string format, IFormatProvider formatProvider)
            => ((int)this).ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode()
            => TypeCode.Int32;

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => Convert.ToBoolean((int)this, provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => Convert.ToByte((int)this, provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => Convert.ToChar((int)this, provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => Convert.ToDateTime((int)this, provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => Convert.ToDecimal((int)this, provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => Convert.ToDouble((int)this, provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => Convert.ToInt16((int)this, provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => Convert.ToInt32((int)this, provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => Convert.ToInt64((int)this, provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => Convert.ToSByte((int)this, provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => Convert.ToSingle((int)this, provider);

        string IConvertible.ToString(IFormatProvider provider)
            => Convert.ToString((int)this, provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => throw new NotSupportedException();

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => Convert.ToUInt16((int)this, provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => Convert.ToUInt32((int)this, provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => Convert.ToUInt64((int)this, provider);
        #endregion
        #region operators
        public static bool operator ==(Int24 left, Int24 right)
            => left.Equals(right);
        public static bool operator !=(Int24 left, Int24 right)
            => !(left == right);
        public static bool operator ==(Int24 left, UInt24 right)
            => left < 0 || right > MaxValue ? false : left == (int)right;
        public static bool operator !=(Int24 left, UInt24 right)
            => !(left == right);
        public static bool operator ==(Int24 left, int right)
            => (int)left == right;
        public static bool operator !=(Int24 left, int right)
            => !(left == right);
        public static bool operator ==(int left, Int24 right)
            => right == left;
        public static bool operator !=(int left, Int24 right)
            => !(right == left);
        public static Int24 operator &(Int24 left, Int24 right)
            => new Int24((int)left & right);
        public static Int24 operator |(Int24 left, Int24 right)
            => new Int24((int)left | right);
        public static Int24 operator ^(Int24 left, Int24 right)
            => new Int24((int)left ^ right);
        public static Int24 operator ~(Int24 int24)
            => new Int24(~(int)int24 & MaxValue);
        #endregion
        #region casts
        // explicit = Narrowing
        // implicit = Widening

        // narrowing casts
        public static explicit operator byte(Int24 int24)
            => int24._short == 0 ? int24._byte : throw new InvalidCastException(nameof(int24));
        public static explicit operator sbyte(Int24 int24)
            => (sbyte)(int)int24;
        public static explicit operator short(Int24 int24)
            => (short)(int)int24;
        public static explicit operator ushort(Int24 int24)
            => (ushort)(int)int24;
        public static explicit operator UInt24(Int24 int24)
            => new UInt24((uint)int24);
        public static explicit operator uint(Int24 int24)
            => (uint)(int)int24;
        public static explicit operator ulong(Int24 int24)
            => (ulong)(int)int24;

        // widening casts
        public static implicit operator int(Int24 int24)
            => int24._byte | (int24._short << 8);

        // Int24 narrowing casts
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

        // Int24 widening casts
        public static implicit operator Int24(byte uInt8)
            => new Int24(uInt8);
        public static implicit operator Int24(sbyte int8)
            => new Int24(int8);
        public static implicit operator Int24(short int16)
            => new Int24(int16);
        public static implicit operator Int24(ushort uInt16)
            => new Int24(uInt16);
        #endregion
    }
}

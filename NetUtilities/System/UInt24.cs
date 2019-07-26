using System.Runtime.InteropServices;
#nullable enable
namespace System
{
    /// <summary>
    /// Represents a 24-bit unsigned integer
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct UInt24 : IEquatable<UInt24>, IComparable<UInt24>, IConvertible, IComparable, IFormattable
    {
        private readonly byte _byte;
        private readonly ushort _ushort;

        public const uint MaxValue = 0xFFFFFF;
        public const uint MinValue = 0;

        public UInt24(byte value)
        {
            _byte = value;
            _ushort = 0;
        }
        public UInt24(uint value)
        {
            EnsureRange(value);

            _byte = (byte)(value & 0xFF);
            _ushort = (ushort)(value >> 8);
        }

        private static void EnsureRange(uint value)
        {
            if (value > MaxValue)
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
        public bool Equals(UInt24 other)
            => _byte == other._byte && _ushort == other._ushort;

        public int CompareTo(UInt24 other)
            => ((int)this).CompareTo(other);

        int IComparable.CompareTo(object obj)
            => obj is UInt24 u24 ? CompareTo(u24) : throw new ArgumentException(nameof(obj));

        public string ToString(string format, IFormatProvider formatProvider)
            => ((int)this).ToString(format, formatProvider);

        TypeCode IConvertible.GetTypeCode()
            => TypeCode.UInt32;

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
        public static bool operator ==(UInt24 left, UInt24 right)
            => left.Equals(right);
        public static bool operator !=(UInt24 left, UInt24 right)
            => !(left == right);
        public static bool operator ==(UInt24 left, Int24 right)
            => right == left;
        public static bool operator !=(UInt24 left, Int24 right)
            => right != left;
        public static UInt24 operator &(UInt24 left, UInt24 right)
            => new UInt24((uint)left & right);
        public static UInt24 operator |(UInt24 left, UInt24 right)
            => new UInt24((uint)left | right);
        public static UInt24 operator ^(UInt24 left, UInt24 right)
            => new UInt24((uint)left ^ right);
        public static UInt24 operator ~(UInt24 int24)
            => new UInt24(~(uint)int24 & MaxValue);
        #endregion
        #region casts
        // explicit = Narrowing
        // implicit = Widening

        // narrowing casts
        public static explicit operator byte(UInt24 uInt24)
            => uInt24._ushort == 0 ? uInt24._byte : throw new InvalidCastException(nameof(uInt24));
        public static explicit operator sbyte(UInt24 uInt24)
            => (sbyte)(int)uInt24;
        public static explicit operator short(UInt24 uInt24)
            => (short)(int)uInt24;
        public static explicit operator ushort(UInt24 uInt24)
            => (ushort)(int)uInt24;
        public static explicit operator Int24(UInt24 uInt24)
            => new Int24(uInt24);

        // widening casts
        public static implicit operator int(UInt24 uInt24)
            => uInt24._byte | (uInt24._ushort << 8);
        public static implicit operator uint(UInt24 uInt24)
            => (uint)(uInt24._byte | (uInt24._ushort << 8));
        public static implicit operator long(UInt24 uInt24)
            => uInt24;

        // UInt24 narrowing casts
        public static implicit operator UInt24(sbyte int8)
            => new UInt24((uint)int8);
        public static implicit operator UInt24(short int16)
            => new UInt24((uint)int16);
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

        // UInt24 widening casts
        public static implicit operator UInt24(byte uInt8)
            => new UInt24(uInt8);
        public static implicit operator UInt24(ushort uInt16)
            => new UInt24(uInt16);
        #endregion
    }
}
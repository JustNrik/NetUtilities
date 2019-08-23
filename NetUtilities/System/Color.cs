namespace System
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// This struct represents a RGB Color
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 3, Pack = 1)]
    public readonly struct Color : IEquatable<Color>, IComparable<Color>, IFormattable, IComparable
    {
        public UInt24 RawValue { get; }
        public byte R => (byte)(RawValue._value >> 16);
        public byte G => (byte)(RawValue._value >> 8);
        public byte B => (byte)RawValue._value;

        public Color(byte r, byte g, byte b)
            => RawValue = (UInt24)((uint)r << 16 | (uint)g << 8 | b);
        public Color(uint value)
        {
            ThrowIfOutOfRange(value);
            RawValue = (UInt24)value;
        }

        private static void ThrowIfOutOfRange(uint value)
        {
            if (value > UInt24.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), 
                    $"The value must be lower than or equal to {UInt24.MaxValue}");
        }

        public Color(UInt24 value)
            => RawValue = value;

        public Color(int r, int g, int b)
        {
            ThrowIfOutOfRangeInt(r, nameof(r));
            ThrowIfOutOfRangeInt(g, nameof(g));
            ThrowIfOutOfRangeInt(b, nameof(b));

            RawValue = (UInt24)((uint)r << 16 | (uint)g << 8 | (uint)b);
        }

        private static void ThrowIfOutOfRangeSingle(float f, string name)
        {
            if (f < 0 || f > 1)
                throw new ArgumentOutOfRangeException(name, "The value must be in the [0.0, 1.0] range");
        }

        private static void ThrowIfOutOfRangeInt(int i, string name)
        {
            if ((uint)i > 255)
                throw new ArgumentOutOfRangeException(name, "The value must be in the [0, 255] range");
        }

        public Color(float r, float g, float b)
        {
            ThrowIfOutOfRangeSingle(r, nameof(r));
            ThrowIfOutOfRangeSingle(g, nameof(g));
            ThrowIfOutOfRangeSingle(b, nameof(b));

            RawValue = (UInt24)(((uint)(r * 255f) << 16) | ((uint)(g * 255f) << 8) | (uint)(b * 255f));
        }

        public Color Combine(Color other)
            => new Color(RawValue | other.RawValue);

        public Color Remove(Color other)
            => new Color(RawValue & other.RawValue);

        public Color Invert()
            => new Color(~RawValue);

        #region other things
        public override bool Equals(object obj)
            => obj is Color color && color.RawValue._value == RawValue._value;

        public override int GetHashCode()
            => (int)RawValue._value;

        public override string ToString()
            => $"#{Convert.ToString(RawValue._value, 16)}";

        public bool Equals(Color color)
            => color.RawValue._value == RawValue._value;
        int IComparable.CompareTo(object obj)
            => obj is Color color ? CompareTo(color) : throw new ArgumentException(nameof(obj));
        public int CompareTo(Color other)
            => RawValue._value.CompareTo(other.RawValue._value);
        public string ToString(string format, IFormatProvider formatProvider)
            => RawValue._value.ToString(format, formatProvider);
        public static bool operator ==(Color left, Color right)
            => left.Equals(right);
        public static bool operator !=(Color left, Color right)
            => !left.Equals(right);
        public static bool operator >(Color left, Color right)
            => left.RawValue._value > right.RawValue._value;
        public static bool operator <(Color left, Color right)
            => left.RawValue._value < right.RawValue._value;
        public static bool operator >=(Color left, Color right)
            => left.RawValue._value >= right.RawValue._value;
        public static bool operator <=(Color left, Color right)
            => left.RawValue._value <= right.RawValue._value;
        #endregion
        #region static fields
        public static Color White   = new Color(0xFFFFFF);
        public static Color Black   = new Color(0x000000);
        public static Color Gray    = new Color(0xC0C0C0);
        public static Color Red     = new Color(0xFF0000);
        public static Color Green   = new Color(0x00FF00);
        public static Color Blue    = new Color(0x0000FF);
        public static Color Cyan    = new Color(0x00FFFF);
        public static Color Yellow  = new Color(0xFFFF00);
        public static Color Magenta = new Color(0xFF00FF);
        #endregion
        #region casts
        public static explicit operator UInt24(Color color)
            => new UInt24(color.RawValue);
        public static explicit operator Color(UInt24 uInt24)
            => new Color(uInt24);
        #endregion
    }
}
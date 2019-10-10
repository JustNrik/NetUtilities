using NetUtilities;
using System.Runtime.InteropServices;

namespace System
{
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
            if (value > UInt24.MaxValue)
                Throw.ArgumentOutOfRange($"The value must be lower than or equal to {UInt24.MaxValue}");

            RawValue = (UInt24)value;
        }

        public Color(UInt24 value)
            => RawValue = value;

        public Color(int r, int g, int b)
        {
            if ((uint)r > 255)
                Throw.ArgumentOutOfRange(nameof(r), "The value must be in the [0, 255] range");

            if ((uint)g > 255)
                Throw.ArgumentOutOfRange(nameof(g), "The value must be in the [0, 255] range");

            if ((uint)b > 255)
                Throw.ArgumentOutOfRange(nameof(b), "The value must be in the [0, 255] range");

            RawValue = (UInt24)((uint)r << 16 | (uint)g << 8 | (uint)b);
        }

        public Color(float r, float g, float b)
        {
            if (r < 0f || r > 1f)
                Throw.ArgumentOutOfRange(nameof(r), "The value must be in the [0.0, 1.0] range");

            if (g < 0f || g > 1f)
                Throw.ArgumentOutOfRange(nameof(g), "The value must be in the [0.0, 1.0] range");

            if (b < 0f || b > 1f)
                Throw.ArgumentOutOfRange(nameof(b), "The value must be in the [0.0, 1.0] range");

            RawValue = (UInt24)(((uint)(r * 255f) << 16) | ((uint)(g * 255f) << 8) | (uint)(b * 255f));
        }

        public Color Combine(Color other)
            => this | other;

        public Color Remove(Color other)
            => this & other;

        public Color Invert()
            => ~this;

        public static Color operator |(Color left, Color right)
            => new Color(left.RawValue | right.RawValue);

        public static Color operator &(Color left, Color right)
            => new Color(left.RawValue & right.RawValue);

        public static Color operator ~(Color color)
            => new Color(~color.RawValue);

        #region other things
        public override bool Equals(object obj)
            => obj is Color color && color.RawValue._value == RawValue._value;

        public override int GetHashCode()
            => (int)RawValue._value;

        public override string ToString()
            => $"#{RawValue._value:X6}";

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
        public static readonly Color White = new Color(0xFFFFFF);
        public static readonly Color Black = new Color(0x000000);
        public static readonly Color Gray = new Color(0xC0C0C0);
        public static readonly Color Red = new Color(0xFF0000);
        public static readonly Color Green = new Color(0x00FF00);
        public static readonly Color Blue = new Color(0x0000FF);
        public static readonly Color Cyan = new Color(0x00FFFF);
        public static readonly Color Yellow = new Color(0xFFFF00);
        public static readonly Color Magenta = new Color(0xFF00FF);
        #endregion
        #region casts
        public static explicit operator UInt24(Color color)
            => color.RawValue;
        public static explicit operator Color(UInt24 uInt24)
            => new Color(uInt24);
        #endregion
    }
}
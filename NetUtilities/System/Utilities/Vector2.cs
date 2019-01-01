namespace System.Utilities
{
    /// <summary>
    /// Vector2
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Distance => DistanceTo(Zero);

        public Vector2(double value)
            => X = Y = value;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b)
            => new Vector2(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator -(Vector2 v)
            => Zero - v;

        public static Vector2 operator *(Vector2 a, Vector2 b)
            => new Vector2(a.X * b.X, a.Y * b.Y);

        public static Vector2 operator /(Vector2 a, Vector2 b)
            => new Vector2(a.X / b.X, a.Y / b.Y);

        public static Vector2 operator %(Vector2 a, Vector2 b)
            => new Vector2(a.X % b.X, a.Y % b.Y);

        public static bool operator ==(Vector2 a, Vector2 b)
            => a.Equals(b);

        public static bool operator !=(Vector2 a, Vector2 b)
            => !a.Equals(b);

        public bool Equals(Vector2 other)
            => X == other.X
            && Y == other.Y;

        public override bool Equals(object obj)
            => obj is Vector2 v ? v.Equals(this) : false;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = X.GetHashCode();

                hashcode = ((hashcode << 5) + hashcode) ^ Y.GetHashCode();

                return hashcode;
            }
        }

        public Vector2 Floor()
            => new Vector2(Math.Floor(X), Math.Floor(Y));

        public Vector2 Ceiling()
            => new Vector2(Math.Ceiling(X), Math.Ceiling(Y));

        public Vector2 Round()
            => new Vector2(Math.Round(X), Math.Round(Y));

        public Vector2 Round(int digits)
            => new Vector2(Math.Round(X, digits), Math.Round(Y, digits));

        public Vector2 Round(MidpointRounding mode)
            => new Vector2(Math.Round(X, mode), Math.Round(Y, mode));

        public Vector2 Round(int digits, MidpointRounding mode)
            => new Vector2(Math.Round(X, digits, mode), Math.Round(Y, digits, mode));

        public Vector2 Max(Vector2 v)
            => new Vector2(Math.Max(X, v.X), Math.Max(Y, v.Y));

        public Vector2 Min(Vector2 v)
            => new Vector2(Math.Min(X, v.X), Math.Min(Y, v.Y));

        public Vector2 Normalize()
            => new Vector2(X / Distance, Y / Distance);

        public double DistanceTo(Vector2 v)
            => Math.Sqrt(Square(X - v.X) + Square(Y - v.Y));

        private static double Square(double x)
            => Math.Pow(x, 2);

        public override string ToString()
            => ToString(null, null);

        public string ToString(string format, IFormatProvider formatProvider)
            => format is null ? $"({X}, {Y})" : formatProvider is null
            ? string.Format(format, X, Y)
            : string.Format(formatProvider, format, X, Y);

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);
    }
}
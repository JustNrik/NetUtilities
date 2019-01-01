namespace System.Utilities
{
    public struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Distance => DistanceTo(Zero);

        public Vector3(double value)
            => X = Y = Z = value;

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
            => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3 operator -(Vector3 a, Vector3 b)
            => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3 operator -(Vector3 v)
            => Zero - v;

        public static Vector3 operator *(Vector3 a, Vector3 b)
            => new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static Vector3 operator /(Vector3 a, Vector3 b)
            => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        public static Vector3 operator %(Vector3 a, Vector3 b)
            => new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static bool operator ==(Vector3 a, Vector3 b)
            => a.Equals(b);

        public static bool operator !=(Vector3 a, Vector3 b)
            => !a.Equals(b);

        public bool Equals(Vector3 other)
            => X == other.X
            && Y == other.Y
            && Z == other.Z;

        public override bool Equals(object obj)
            => obj is Vector3 v ? v.Equals(this) : false;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = X.GetHashCode();

                hashcode = ((hashcode << 5) + hashcode) ^ Y.GetHashCode();
                hashcode = ((hashcode << 5) + hashcode) ^ Z.GetHashCode();

                return hashcode;
            }
        }

        public Vector3 Floor()
            => new Vector3(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));

        public Vector3 Ceiling()
            => new Vector3(Math.Ceiling(X), Math.Ceiling(Y), Math.Ceiling(Z));

        public Vector3 Round()
            => new Vector3(Math.Round(X), Math.Round(Y), Math.Round(Z));

        public Vector3 Round(int digits)
            => new Vector3(Math.Round(X, digits), Math.Round(Y, digits), Math.Round(Z, digits));

        public Vector3 Round(MidpointRounding mode)
            => new Vector3(Math.Round(X, mode), Math.Round(Y, mode), Math.Round(Z, mode));

        public Vector3 Round(int digits, MidpointRounding mode)
            => new Vector3(Math.Round(X, digits, mode), Math.Round(Y, digits, mode), Math.Round(Z, digits, mode));

        public Vector3 Max(Vector3 v)
            => new Vector3(Math.Max(X, v.X), Math.Max(Y, v.Y), Math.Max(Z, v.Z));

        public Vector3 Min(Vector3 v)
            => new Vector3(Math.Min(X, v.X), Math.Min(Y, v.Y), Math.Min(Z, v.Z));

        public Vector3 Normalize()
            => new Vector3(X / Distance, Y / Distance, Z / Distance);

        public double DistanceTo(Vector3 v)
            => Math.Sqrt(Square(X - v.X) + Square(Y - v.Y) + Square(Z - v.Z));

        private static double Square(double x)
            => Math.Pow(x, 2);

        public override string ToString()
            => ToString(null, null);

        public string ToString(string format, IFormatProvider formatProvider)
            => format is null ? $"({X}, {Y}, {Z})" : formatProvider is null
            ? string.Format(format, X, Y, Z) 
            : string.Format(formatProvider, format, X, Y, Z);

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
    }
}

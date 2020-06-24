using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetUtilities;

namespace System.Collections.Generic
{
    public sealed class EqualityComparer64<T> : IEqualityComparer64<T>
    {
        public static IEqualityComparer64<T> Default { get; } = new EqualityComparer64<T>();

        private EqualityComparer64() 
        {
        }

        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x is IEquatable<T> equatable
                ? equatable.Equals(y)
                : x.Equals(y);
        }

        public long GetHashCode64([DisallowNull] T obj)
        {
            const ulong hash = 3074457345618258799UL;

            if (typeof(T) == typeof(long))
                return (long)(object)obj;
            else if (typeof(T) == typeof(ulong))
                return (long)(ulong)(object)obj;
            else if (typeof(T) == typeof(int))
                return (int)(object)obj;
            else if (typeof(T) == typeof(uint))
                return (uint)(object)obj;
            else if (typeof(T) == typeof(short))
                return (short)(object)obj;
            else if (typeof(T) == typeof(ushort))
                return (ushort)(object)obj;
            else if (typeof(T) == typeof(byte))
                return (byte)(object)obj;
            else if (typeof(T) == typeof(sbyte))
                return (sbyte)(object)obj;
            else if (typeof(T) == typeof(char))
                return (char)(object)obj;
            else if (typeof(T) == typeof(nint))
                return (nint)(object)obj;
            else if (typeof(T) == typeof(nuint))
                return (long)(nuint)(object)obj;
            else if (typeof(T) == typeof(bool))
                return (bool)(object)obj ? 1 : 0;

            if (obj is IEquatable64<T> equatable)
                return equatable.GetHashCode64();

            if (typeof(T).IsValueType)
            {
                var hashCode = 3074457345618258791UL;
                Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), obj);

                foreach (var @byte in span)
                    hashCode += (hashCode + @byte) * hash;

                return (long)hashCode;
            }

            if (obj is string str)
            {
                var intern = string.Intern(str);
                return Unsafe.As<string, long>(ref intern);
            }

            return Unsafe.As<T, long>(ref obj);
        }
    }
}
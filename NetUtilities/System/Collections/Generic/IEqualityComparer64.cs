using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetUtilities;

namespace System.Collections.Generic
{
    public interface IEqualityComparer64<in T>
    {
        bool Equals([AllowNull] T x, [AllowNull] T y);
        long GetHashCode64([DisallowNull] T obj);
    }

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

            var hashCode = 3074457345618258791UL;

            if (obj is IEquatable64<T> equatable)
                return equatable.GetHashCode64();

            if (obj.IsUnmanaged())
            {
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

            if (typeof(T).IsValueType)
            {
                if (typeof(T) == typeof(long))
                    return (long)(object)obj;

                if (typeof(T) == typeof(ulong))
                    return (long)(ulong)(object)obj;

                return (long)obj.GetHashCode();
            }

            return Unsafe.As<T, long>(ref obj);
        }
    }
}
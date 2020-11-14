using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    public interface IEqualityComparer64<in T>
    {
        bool Equals([AllowNull] T x, [AllowNull] T y);
        long GetHashCode64([DisallowNull] T obj);
    }
}
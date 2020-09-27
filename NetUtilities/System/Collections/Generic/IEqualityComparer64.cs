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
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetUtilities;

namespace System
{
    public static class RandomExtensions 
    {
        public static T Next<T>(this Random random) 
            where T : unmanaged
        {
            Ensure.NotNull(random);

            var result = default(T);
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, Unsafe.SizeOf<T>()));

            random.NextBytes(span);
            return result;
        }
    }
}

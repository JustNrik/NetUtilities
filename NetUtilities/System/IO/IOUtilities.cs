using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IOUtilities
    {
        public static T Read<T>(this Stream stream) where T : struct
        {
            Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];

            stream.Read(span);
            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(span));
        }

        public static void Write<T>(this Stream stream, T value) where T : struct
        {
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1));
            stream.Write(span);
        }
    }
}

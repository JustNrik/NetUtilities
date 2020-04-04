using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.IO
{
    /// <summary>
    /// Utility class for <see cref="IO"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IOExtensions
    {
        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// This is equal to the size of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to be read.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>A value.</returns>
        public static T Read<T>(this Stream stream) where T : struct
        {
            Span<byte> span = stackalloc byte[Unsafe.SizeOf<T>()];

            stream.Read(span);
            return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(span));
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// This is equal to the size of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to be written</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        public static void Write<T>(this Stream stream, T value) where T : struct
        {
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1));
            stream.Write(span);
        }
    }
}

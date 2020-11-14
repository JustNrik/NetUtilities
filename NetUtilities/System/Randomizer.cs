using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    /// <inheritdoc/>
    public class Randomizer : Random
    {
        /// <summary>
        ///     Gets a shared <see cref="Randomizer"/> instance.
        /// </summary>
        public static Randomizer Shared { get; } = new Randomizer();

        /// <summary>
        ///     Generates a random value of the provided type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <returns>
        ///     A random value of the provided type.
        /// </returns>
        public T Next<T>() where T : unmanaged
        {
            var result = default(T);
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, Unsafe.SizeOf<T>()));

            NextBytes(span);
            return result;
        }
    }
}

using NetUtilities;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    /// <inheritdoc/>
    public class Randomizer : Random
    {
        /// <summary>
        /// Returns a singleton instance of a <see cref="Randomizer"/>.
        /// </summary>
        public static Randomizer Shared { get; } = new Randomizer();

        /// <summary>
        /// Returns a non-negative random 64-bits integer.
        /// </summary>
        /// <returns>A non-negative random 64-bits integer.</returns>
        public virtual long NextLong()
            => (long)(Sample() * long.MaxValue);

        /// <summary>
        /// Returns a non-negative random 64-bits integer that is lower than <paramref name="max"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="max"/> is lower than zero.</exception>
        /// <param name="max">The upper bound.</param>
        /// <returns>A non-negative random 64-bits integer that is lower than <paramref name="max"/>.</returns>
        public virtual long NextLong(long max)
        {
            Ensure.NotOutOfRange(max < 0, max);
            return (long)(Sample() * max);
        }

        /// <summary>
        /// Returns a random 64-bits integer that is higher or equal than <paramref name="min"/> and lower than <paramref name="max"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="max"/> is lower than <paramref name="min"/>.</exception>
        /// <param name="min">The lower bound.</param>
        /// <param name="max">The upper bound.</param>
        /// <returns>A random 64-bits integer that is higher or equal than <paramref name="min"/> and lower than <paramref name="max"/>.</returns>
        public virtual long NextLong(long min, long max)
        {
            Ensure.NotOutOfRange(max < min, max);

            var sample = Sample();
            // This is slightly slower than Sample() * (max - min)
            // But it won't return a value outside the range and handles all long ranges (like long.MinValue to long.MaxValue)
            return min + (long)(sample * max - sample * min);
        }

        /// <inheritdoc/>
        public override int Next(int min, int max)
        {
            Ensure.NotOutOfRange(max < min, max);
            return min + (int)(Sample() * ((long)max - min));
        }

        /// <summary>
        /// Returns a random value of the <typeparamref name="T"/> <see langword="unmanaged struct"/>.
        /// </summary>
        /// <typeparam name="T">The <see langword="unmanaged struct"/>.</typeparam>
        /// <returns>A random value of the <typeparamref name="T"/> <see langword="unmanaged struct"/>.</returns>
        public T Next<T>() where T : unmanaged
        {
            var result = default(T);
            var span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, Unsafe.SizeOf<T>()));

            NextBytes(span);
            return result;
        }
    }
}

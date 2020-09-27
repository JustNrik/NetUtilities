using System.Runtime.CompilerServices;
using NetUtilities;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System
{
    /// <summary>
    ///     This class provides some math helper methods that <see cref="Math"/> doesn't provide.
    /// </summary>
    public static class MathX
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        ///     The maximum value you can provide to <see cref="SquareSum(uint)"/>
        /// </summary>
        public const uint SquareSumThreshold = 2343;

        /// <summary>
        ///     The maximum value you can provide to <see cref="CubicSum(uint)"/>
        /// </summary>
        public const uint CubicSumThreshold = 361;

        /// <summary>
        ///     Returns the sum of all integers from 1 to <paramref name="n"/>.
        /// </summary>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of all integers from 1 to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static uint Sum(uint n)
        {
            var result = (n + 1UL) * n / 2;

            if (result > uint.MaxValue)
                throw new OverflowException($"The sum from 1 to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");

            return (uint)result;
        }

        /// <summary>
        ///     Returns the sum of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </summary>
        /// <param name="k">
        ///     The lower limit of the numbers to be sum.
        /// </param>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="k"/> is higher than <paramref name="n"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static uint Sum(uint k, uint n)
        {
            Ensure.CanOperate(k <= n, $"The value of k ({k}) must be lower than the value of n ({n})");

            if (k == n)
                return n;

            if (k <= 1u)
                return Sum(n);

            var result = (1UL + n - k) * (n + k) / 2;

            if (result > uint.MaxValue)
                throw new OverflowException($"The sum from {k} to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");

            return (uint)result;
        }

        /// <summary>
        ///     Returns the sum of the squares of all integers from 1 to <paramref name="n"/>.
        /// </summary>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of the squares of all integers from 1 to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/> (<paramref name="n"/> >= 2344)
        /// </exception>
        [MethodImplementation(Inlined)]
        public static uint SquareSum(uint n)
        {
            if (n > SquareSumThreshold)
                throw new OverflowException($"The square sum from 1 to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");

            var result = (n + 1UL) * (2 * n + 1) * n / 6;
            return (uint)result;
        }

        /// <summary>
        ///     Returns the sum of the squares of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </summary>
        /// <param name="k">
        ///     The lower limit of the numbers to be sum.
        /// </param>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of the squares of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="k"/> is higher than <paramref name="n"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/>.
        /// </exception>
        public static uint SquareSum(uint k, uint n)
        {
            Ensure.CanOperate(k <= n, $"The value of k ({k}) must be lower than the value of n ({n})");

            ulong result;

            if (k == n)
            {
                result = (ulong)n * n;

                if (result > uint.MaxValue)
                    throw new OverflowException($"The square sum from {k} to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");

                return (uint)result;
            }

            if (k <= 1)
                return SquareSum(n);

            try
            {
                result = checked((1UL + n - k) * ((2 * k * k) + (2 * k * n) - k + (2 * n * n) + n) / 6);

                if (result > uint.MaxValue)
                    throw new OverflowException(); // I know this is slow but needed to properly show the result value.

                return (uint)result;
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The square sum from {k} to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");
            }
        }

        /// <summary>
        ///     Returns the sum of the cubes of all integers from 1 to <paramref name="n"/>.
        /// </summary>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of the cubes of all integers from 1 to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/> (<paramref name="n"/> >= 362).
        /// </exception>
        [MethodImplementation(Inlined)]
        public static uint CubicSum(uint n)
        {
            if (n > CubicSumThreshold)
                throw new OverflowException($"The cubic sum from 1 to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");

            var result = (n + 1UL) * (n + 1) * n * n / 4;

            if (result > uint.MaxValue)
                throw new OverflowException($"The cubic sum of from 1 to {n} resulted into {result} which is bigger than uint.MaxValue ({uint.MaxValue})");

            return (uint)result;
        }

        /// <summary>
        ///     Returns the sum of the cubes of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </summary>
        /// <param name="k">
        ///     The lower limit of the numbers to be sum.
        /// </param>
        /// <param name="n">
        ///     The upper limit of the numbers to be sum.
        /// </param>
        /// <returns>
        ///     The sum of the cubes of all integers from <paramref name="k"/> to <paramref name="n"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="k"/> is higher than <paramref name="n"/>.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     Thrown when the result would be higher than <see cref="uint.MaxValue"/>.
        /// </exception>
        public static uint CubicSum(uint k, uint n)
        {
            Ensure.CanOperate(k <= n, $"The value of k ({k}) must be lower than the value of n ({n})");

            ulong result;

            if (k == n)
            {
                result = (ulong)n * n * n;

                if (result > uint.MaxValue)
                    throw new OverflowException($"The cubic sum of {n} resulted into {result} which is bigger than uint.MaxValue ({uint.MaxValue})");

                return (uint)result;
            }

            if (k <= 1)
                return CubicSum(n);

            try
            {
                result = checked((1UL + n - k) * (k + n) * ((k * k) - k + (n * n) + n) / 4);

                if (result > uint.MaxValue)
                    throw new OverflowException(); // I know this is slow but needed to properly show the result value.

                return (uint)result;
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The cubic sum from {k} to {n} resulted a value larger than uint.MaxValue ({uint.MaxValue})");
            }
        }
    }
}

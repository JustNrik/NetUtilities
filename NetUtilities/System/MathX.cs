using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.System
{
    public static class MathX
    {
        private const MethodImplOptions NoInlining = MethodImplOptions.NoInlining;

        /// <summary>
        /// Returns the sum of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of all integers from 1 to N.</returns>
        public static uint Sum(uint n)
            => n * (n + 1) / 2;

        /// <summary>
        /// Returns the sum of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when k is higher than n.</exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of all integers from K to N.</returns>
        public static uint Sum(uint k, uint n)
        {
            EnsureLowerOrEqual(k, n);

            if (k == n)
                return n;

            if (k == 1)
                return Sum(n);

            return Sum(n) - Sum(k - 1);
        }

        /// <summary>
        /// Returns the sum of the squares of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the squares of all integers from 1 to N.</returns>
        public static uint SquareSum(uint n)
            => n * (n + 1) * (2 * n + 1) / 6;

        /// <summary>
        /// Returns the sum of the squares of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when k is higher than n.</exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the squares of all integers from 1 to N.</returns>
        public static uint SquareSum(uint k, uint n)
        {
            EnsureLowerOrEqual(k, n);

            if (k == n)
                return (uint)Math.Pow(n, 2);

            if (k == 1)
                return SquareSum(n);

            return SquareSum(n) - SquareSum(k - 1);
        }

        /// <summary>
        /// Returns the sum of the cubes of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the cubes of all integers from 1 to N.</returns>
        public static uint CubicSum(uint n)
            => n * n * (n + 1) * (n + 1) / 4;

        /// <summary>
        /// Returns the sum of the cubes of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when k is higher than n.</exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the cubes of all integers from 1 to N.</returns>
        public static uint CubicSum(uint k, uint n)
        {
            EnsureLowerOrEqual(k, n);

            if (k == n)
                return (uint)Math.Pow(n, 3);

            if (k == 1)
                return CubicSum(n);

            return CubicSum(n) - CubicSum(k - 1);
        }

        [MethodImplementation(NoInlining)]
        private static void EnsureLowerOrEqual(uint k, uint n)
        {
            if (k > n)
                throw new InvalidOperationException($"k ({k}) must be lower or equal than n ({n})");
        }
    }
}

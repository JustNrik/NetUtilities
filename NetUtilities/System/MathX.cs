using NetUtilities;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System
{
    public static class MathX
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        /// Returns the sum of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of all integers from 1 to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint Sum(uint n)
            => n * (n + 1u) / 2u;

        /// <summary>
        /// Returns the sum of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">The exception that is thrown when the value of k you provide is higher than the value of n</exception>
        /// <exception cref="OverflowException">
        /// The exception that is thrown when an arithmetic, casting, or conversion operation in a checked context results in an overflow.
        /// </exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of all integers from K to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint Sum(uint k, uint n)
        {
            if (k > n)
                Throw.InvalidOperation($"The value of k ({k}) must be lower than the value of n ({n})");

            if (k == n)
                return n;

            if (k <= 1u)
                return Sum(n);

            return checked((1 + n - k) * (n + k) / 2u);
        }

        /// <summary>
        /// Returns the sum of the squares of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the squares of all integers from 1 to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint SquareSum(uint n)
            => n * (n + 1u) * (2u * n + 1) / 6u;

        /// <summary>
        /// Returns the sum of the squares of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">The exception that is thrown when the value of k you provide is higher than the value of n</exception>
        /// <exception cref="OverflowException">
        /// The exception that is thrown when an arithmetic, casting, or conversion operation in a checked context results in an overflow.
        /// </exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the squares of all integers from 1 to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint SquareSum(uint k, uint n)
        {
            if (k > n)
                Throw.InvalidOperation($"The value of k ({k}) must be lower than the value of n ({n})");

            if (k == n)
                return n * n;

            if (k <= 1)
                return SquareSum(n);

            return checked((1u + n - k) * ((2u * k * k) + (2u * k * n) - k + (2u * n * n) + n) / 6u);
        }

        /// <summary>
        /// Returns the sum of the cubes of all integers from 1 to N.
        /// </summary>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the cubes of all integers from 1 to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint CubicSum(uint n)
            => n * n * (n + 1u) * (n + 1u) / 4u;

        /// <summary>
        /// Returns the sum of the cubes of all integers from K to N.
        /// </summary>
        /// <exception cref="InvalidOperationException">The exception that is thrown when the value of k you provide is higher than the value of n</exception>
        /// <exception cref="OverflowException">
        /// The exception that is thrown when an arithmetic, casting, or conversion operation in a checked context results in an overflow.
        /// </exception>
        /// <param name="k">The lower limit of the numbers to be sum.</param>
        /// <param name="n">The upper limit of the numbers to be sum.</param>
        /// <returns>The sum of the cubes of all integers from 1 to N.</returns>
        [MethodImplementation(Inlined)]
        public static uint CubicSum(uint k, uint n)
        {
            if (k > n)
                Throw.InvalidOperation($"The value of k ({k}) must be lower than the value of n ({n})");

            if (k == n)
                return n * n * n;

            if (k <= 1)
                return CubicSum(n);

            return checked ((1u + n - k) * (k + n) * ((k * k) - k + (n * n) + n) / 4u);
        }
    }
}

using System.Runtime.CompilerServices;
using static System.Math;

namespace System
{
    public static class MathExtensions
    {
        public static decimal RoundToSignificantDigits(this decimal d, int digits)
        {
            if (d is 0)
                return 0;

            var scale = Pow(10, Floor(Log(Abs(d))) + 1);
            return scale * Round(d / scale, digits);
        }

        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d is 0)
                return 0;

            var scale = Math.Pow(10, Floor(Log10(Abs(d))) + 1);
            return scale * Round(d / scale, digits);
        }

        public static float RoundToSignificantDigits(this float s, int digits)
        {
            if (s is 0)
                return 0;

            var scale = MathF.Pow(10, MathF.Floor(MathF.Log10(Abs(s))) + 1);
            return scale * MathF.Round(s / scale, digits);
        }

        public static decimal PowN(decimal value, int power)
        {
            if (power == decimal.Zero) return decimal.One;
            if (power < decimal.Zero) return PowN(decimal.One / value, -power);

            var q = power;
            var prod = decimal.One;
            var current = value;
            while (q > 0)
            {
                if (q % 2 is 1)
                {
                    // detects the 1s in the binary expression of power
                    prod = current * prod; // picks up the relevant power
                    q--;
                }
                current *= current; // value^i -> value^(2*i)
                q /= 2;
            }

            return prod;
        }

        public static decimal Pow(decimal value, decimal pow)
        {
            if (pow == decimal.Zero) return decimal.One;
            if (pow == decimal.One) return value;
            if (value == decimal.One) return decimal.One;

            if (value == decimal.Zero && pow == decimal.Zero) return decimal.One;

            if (value == decimal.Zero)
            {
                if (pow > decimal.Zero)
                    return decimal.Zero;


                throw new InvalidOperationException("Zero base and negative power");
            }

            if (pow == decimal.MinusOne) return decimal.One / value;

            var isPowerInteger = IsInteger(pow);
            if (value < decimal.Zero && !isPowerInteger)
            {
                throw new InvalidOperationException("Negative base and non-integer power");
            }

            if (isPowerInteger && value > decimal.Zero)
            {
                int powerInt = (int)(pow);
                return PowN(value, powerInt);
            }

            if (isPowerInteger && value < decimal.Zero)
            {
                int powerInt = (int)pow;
                if (powerInt % 2 is 0)
                    return Exp(pow * Log(-value));
                else
                    return -Exp(pow * Log(-value));

            }

            return Exp(pow * Log(value));
        }

        private static bool IsInteger(decimal value)
            => Abs(value - (long)value) <= 0.0000000000000000001M;

        public static decimal Exp(decimal x)
        {
            var count = 0;

            while (x > decimal.One)
            {
                x--;
                count++;
            }

            while (x < decimal.Zero)
            {
                x++;
                count--;
            }

            var iteration = 1;
            var result = decimal.One;
            var fatorial = decimal.One;
            decimal cachedResult;

            do
            {
                cachedResult = result;
                fatorial *= x / iteration++;
                result += fatorial;
            } while (cachedResult != result);

            if (count != 0) result *= PowN(E, count);

            return result;
        }

        private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678M;
        private const decimal E = 2.7182818284590452353602874713526624977572470936999595749M;

        public static decimal Log(decimal x)
        {
            if (x <= decimal.Zero)
            {
                throw new ArgumentException("x must be greater than zero");
            }
            var count = 0;
            while (x >= decimal.One)
            {
                x *= Einv;
                count++;
            }
            while (x <= Einv)
            {
                x *= E;
                count--;
            }
            x--;
            if (x == 0) return count;
            var result = decimal.Zero;
            var iteration = 0;
            var y = decimal.One;
            var cacheResult = result - decimal.One;
            while (cacheResult != result && iteration < 100)
            {
                iteration++;
                cacheResult = result;
                y *= -x;
                result += y / iteration;
            }
            return count - result;
        }


        // I tested this against Math.Log method, this eye-cancer thing is by far the fastest.
        // Inlining it makes it 3 times faster. (enjoy these seven nanoseconds)
        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this uint value)
            => GetDigits((ulong)value);

        /// <summary>
        /// Gets the amount of difits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this int value)
            => value == int.MinValue ? 10 : GetDigits((ulong)Math.Abs(value));

        /// <summary>
        /// Gets the amount of difits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this long value)
            => value == long.MinValue ? 19 : GetDigits((ulong)Math.Abs(value));

        /// <summary>
        /// Gets the amount of difits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDigits(this ulong value)
        {
            if (value < 10UL) return 1;
            if (value < 100UL) return 2;
            if (value < 1000UL) return 3;
            if (value < 10000UL) return 4;
            if (value < 100000UL) return 5;
            if (value < 1000000UL) return 6;
            if (value < 10000000UL) return 7;
            if (value < 100000000UL) return 8;
            if (value < 1000000000UL) return 9;
            if (value < 10000000000UL) return 10;
            if (value < 100000000000UL) return 11;
            if (value < 1000000000000UL) return 12;
            if (value < 10000000000000UL) return 13;
            if (value < 100000000000000UL) return 14;
            if (value < 1000000000000000UL) return 15;
            if (value < 10000000000000000UL) return 16;
            if (value < 100000000000000000UL) return 17;
            if (value < 1000000000000000000UL) return 18;
            if (value < 10000000000000000000UL) return 19;
            return 20;
        }
    }
}

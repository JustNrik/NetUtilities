using NetUtilities;
using System.Runtime.CompilerServices;
using static System.Math;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System
{
    public static class MathExtensions
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;
        private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678M;
        private const decimal E = 2.7182818284590452353602874713526624977572470936999595749M;

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
            if (power is 0) return 1m;
            if (power < 0m) return PowN(1m / value, -power);

            var q = power;
            var prod = 1m;
            var current = value;
            while (q > 0)
            {
                if (q.IsOdd())
                {
                    prod = current * prod;
                    q--;
                }
                current *= current;
                q /= 2;
            }

            return prod;
        }

        public static decimal Pow(decimal value, decimal pow)
        {
            if (pow is 0m || value is 1m) return 1m;
            if (pow is 1m) return value;

            if (value is 0m)
            {
                if (pow > 0m)
                    return 0m;


                Throw.InvalidOperation("Zero base and negative power");
            }

            if (pow is -1m) return 1m / value;

            if (IsInteger(pow))
            {
                var intPow = (int)pow;

                if (value > 0m)
                    return PowN(value, intPow);
                else
                {
                    if (intPow.IsEven())
                        return Exp(pow * Log(-value));
                    else
                        return -Exp(pow * Log(-value));
                }
            }

            if (value < 0m)
                throw new InvalidOperationException("Negative base and non-integer power");

            return Exp(pow * Log(value));
        }

        private static bool IsInteger(in decimal value)
            => Abs(value - (long)value) <= 0.0000000000000000001M;

        public static decimal Exp(decimal x)
        {
            var count = 0;

            while (x > 1m)
            {
                x--;
                count++;
            }

            while (x < 0m)
            {
                x++;
                count--;
            }

            var iteration = 1;
            var result = 1m;
            var fatorial = 1m;

            decimal cachedResult;

            do
            {
                cachedResult = result;
                fatorial *= x / iteration++;
                result += fatorial;
            } while (cachedResult != result);

            if (!(count is 0)) 
                result *= PowN(E, count);

            return result;
        }

        public static decimal Log(decimal x)
        {
            if (x <= 0m)
                throw new ArgumentException("x must be greater than zero");

            var count = 0;

            while (x >= 1m)
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

            if (x is 0m) 
                return count;

            var result = 0m;
            var iteration = 0;
            var y = 1m;
            var cacheResult = result - 1m;

            while (cacheResult != result && iteration < 100)
            {
                iteration++;
                cacheResult = result;
                y *= -x;
                result += y / iteration;
            }

            return count - result;
        }

        [MethodImplementation(Inlined)]
        public static bool IsEven(this int value)
            => (value & 1) is 0;

        [MethodImplementation(Inlined)]
        public static bool IsEven(this uint value)
            => (value & 1) is 0;

        [MethodImplementation(Inlined)]
        public static bool IsEven(this long value)
            => (value & 1) is 0;

        [MethodImplementation(Inlined)]
        public static bool IsEven(this ulong value)
            => (value & 1) is 0;

        [MethodImplementation(Inlined)]
        public static bool IsOdd(this int value)
            => (value & 1) is 1;

        [MethodImplementation(Inlined)]
        public static bool IsOdd(this uint value)
            => (value & 1) is 1;

        [MethodImplementation(Inlined)]
        public static bool IsOdd(this long value)
            => (value & 1) is 1;

        [MethodImplementation(Inlined)]
        public static bool IsOdd(this ulong value)
            => (value & 1) is 1;

        /// <summary>
        /// Returns a boolean indicating if the provided number is a power of 2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsPowerOf2(this int value)
            => value < 2 ? false : IsPowerOf2((ulong)value);

        /// <summary>
        /// Returns a boolean indicating if the provided number is a power of 2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsPowerOf2(this uint value)
            => IsPowerOf2((ulong)value);

        /// <summary>
        /// Returns a boolean indicating if the provided number is a power of 2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsPowerOf2(this long value)
            => value < 2 ? false : IsPowerOf2((ulong)value);

        /// <summary>
        /// Returns a boolean indicating if the provided number is a power of 2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsPowerOf2(this ulong value)
            => value < 2 ? false : ((value & (value - 1)) == 0);

        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static int GetDigits(this uint value)
            => GetDigits((ulong)value);

        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static int GetDigits(this int value)
            => value == int.MinValue ? 10 : GetDigits((ulong)Abs(value));

        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static int GetDigits(this long value)
            => value == long.MinValue ? 19 : GetDigits((ulong)Abs(value));

        /// <summary>
        /// Gets the amount of digits in the provided number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static int GetDigits(this ulong value)
        {
            if (value < _8)
            {
                if (value < _5)
                {
                    if (value < _3)
                    {
                        if (value < _2)
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        if (value < _4)
                        {
                            return 3;
                        }
                        else
                        {
                            return 4;
                        }
                    }
                }
                else
                {
                    if (value < _7)
                    {
                        if (value < _6)
                        {
                            return 5;
                        }
                        else
                        {
                            return 6;
                        }
                    }
                    else
                    {
                        return 7;
                    }
                }
            }
            else if (value < _16)
            {
                if (value < _12)
                {
                    if (value < _10)
                    {
                        if (value < _9)
                        {
                            return 8;
                        }
                        else
                        {
                            return 9;
                        }
                    }
                    else
                    {
                        if (value < _11)
                        {
                            return 10;
                        }
                        else
                        {
                            return 11;
                        }
                    }
                }
                else
                {
                    if (value < _14)
                    {
                        if (value < _13)
                        {
                            return 12;
                        }
                        else
                        {
                            return 13;
                        }
                    }
                    else
                    {
                        if (value < _15)
                        {
                            return 14;
                        }
                        else
                        {
                            return 15;
                        }
                    }
                }
            }
            else
            {
                if (value < _18)
                {
                    if (value < _17)
                    {
                        return 16;
                    }
                    else
                    {
                        return 17;
                    }
                }
                else
                {
                    if (value < _19)
                    {
                        return 18;
                    }
                    else if (value < _20)
                    {
                        return 19;
                    }
                    else
                    {
                        return 20;
                    }
                }
            }
        }

        private const ulong _2 = 10UL;
        private const ulong _3 = 100UL;
        private const ulong _4 = 1000UL;
        private const ulong _5 = 10000UL;
        private const ulong _6 = 100000UL;
        private const ulong _7 = 1000000UL;
        private const ulong _8 = 10000000UL;
        private const ulong _9 = 100000000UL;
        private const ulong _10 = 1000000000UL;
        private const ulong _11 = 10000000000UL;
        private const ulong _12 = 100000000000UL;
        private const ulong _13 = 1000000000000UL;
        private const ulong _14 = 10000000000000UL;
        private const ulong _15 = 100000000000000UL;
        private const ulong _16 = 1000000000000000UL;
        private const ulong _17 = 10000000000000000UL;
        private const ulong _18 = 100000000000000000UL;
        private const ulong _19 = 1000000000000000000UL;
        private const ulong _20 = 10000000000000000000UL;
    }
}

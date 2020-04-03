using System.Collections.Generic;

namespace System
{
    public readonly struct RandomComparer<T> : IComparer<T>
    {
        private static readonly Random _random = new Random();

        public int Compare(T x, T y)
            => _random.Next(-1, 2);
    }
}

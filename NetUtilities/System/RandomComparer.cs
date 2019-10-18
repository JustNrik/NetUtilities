using System.Collections.Generic;

namespace System
{
    public class RandomComparer<T> : IComparer<T>
    {
        public static RandomComparer<T> Instance = new RandomComparer<T>();

        protected static readonly Random _random = new Random();

        private RandomComparer() 
        {
        }

        public virtual int Compare(T x, T y)
            => _random.Next(-1, 2);
    }
}

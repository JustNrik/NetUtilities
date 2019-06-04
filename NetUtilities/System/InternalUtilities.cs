using System;
using System.Collections.Generic;

namespace NetUtilities
{
    public static class Ensure
    {
        public static void NotNull(object obj, string name)
        {
            if (obj is null)
                throw new ArgumentNullException(name);
        }

        public static void ZeroOrPositive(int number, string name)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(name);
        }

        public static void Positive(int number, string name)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(name);
        }

        public static void CanWrite<TKey, TValue>(IDictionary<TKey, TValue> source)
        {
            if (source.IsReadOnly)
                throw new InvalidOperationException("The source is a Read-Only dictionary");
        }

        public static void CanWrite<T>(IList<T> source)
        {
            if (source.IsReadOnly)
                throw new InvalidOperationException("The source is a Read-Only list");
        }

        public static void IndexInRange(int index, int count)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
        }

        public static void ValidCount(int index, int count, int maxCount)
        {
            if (index + count > maxCount)
                throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
}

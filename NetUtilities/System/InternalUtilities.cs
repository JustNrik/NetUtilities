﻿namespace NetUtilities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

    public static class Ensure
    {
        private const MethodImplOptions NotInlined = MethodImplOptions.NoInlining;

        [MethodImplementation(NotInlined)]
        public static T NotNull<T>(T obj, string name)
        {
            if (obj is null)
                throw new ArgumentNullException(name);

            return obj;
        }

        [MethodImplementation(NotInlined)]
        public static void ZeroOrPositive(int number, string name)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImplementation(NotInlined)]
        public static void Positive(int number, string name)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImplementation(NotInlined)]
        public static void CanWrite<T>(ICollection<T> source)
        {
            if (source.IsReadOnly)
                throw new InvalidOperationException($"{typeof(T).Name} is a Read-Only collection");
        }

        [MethodImplementation(NotInlined)]
        public static void IndexInRange(int index, int count)
        {
            if ((uint)index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
        }

        [MethodImplementation(NotInlined)]
        public static void ValidCount(int index, int count, int maxCount)
        {
            if (index + count > maxCount)
                throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
}

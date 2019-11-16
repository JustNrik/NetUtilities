using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace NetUtilities
{
    internal static class Throw
    {
        private const MethodImplOptions NotInlined = MethodImplOptions.NoInlining;

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void InvalidOperation(string message)
        {
            throw new InvalidOperationException(message);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void NullArgument(string argumentName)
        {
            throw new ArgumentNullException(argumentName);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void InvalidFormat(string message)
        {
            throw new FormatException(message);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void Overflow(string message)
        {
            throw new OverflowException(message);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void ArgumentOutOfRange(string argumentName)
        {
            throw new ArgumentOutOfRangeException(argumentName);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void ArgumentOutOfRange(string argumentName, string message)
        {
            throw new ArgumentOutOfRangeException(argumentName, message);
        }

        [DoesNotReturn, MethodImplementation(NotInlined)]
        public static void ParameterCountMismatch(string message)
        {
            throw new ParameterCountMismatchException(message);
        }
    }
    internal static class Ensure
    {
        private const MethodImplOptions NotInlined = MethodImplOptions.NoInlining;

        [MethodImplementation(NotInlined)]
        [return: NotNull]
        public static T NotNull<T>(T obj, string name)
        {
            if (obj is null)
                throw new ArgumentNullException(name);

            return obj;
        }

        [MethodImplementation(NotInlined)]
        public static void Positive(int number, string name)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(name);
        }

        [MethodImplementation(NotInlined)]
        public static void IndexInRange(int index, int count)
        {
            if ((uint)index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}

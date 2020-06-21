using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace NetUtilities
{
    internal static class Ensure
    {
        private const MethodImplOptions NotInlined = MethodImplOptions.NoInlining;

        [MethodImplementation(NotInlined)]
        [return: NotNull]
        public static T NotNull<T>([AllowNull]T obj, [CallerArgumentExpression("obj")]string? name = null)
        {
            if (obj is null)
                throw new ArgumentNullException(name);

            return obj;
        }

        public static void NotOutOfRange<T>(
            [DoesNotReturnIf(false)]bool rangeCondition, T value,
            [CallerArgumentExpression("rangeCondition")]string? expression = null,
            [CallerArgumentExpression("value")]string? argumentName = null)
        {
            if (!rangeCondition)
                throw new ArgumentOutOfRangeException(
                    argumentName, value, $"The expression '{expression}' has a value that doesn't meet the range condition");
        }

        public static void NotReadOnly([DoesNotReturnIf(true)]bool isReadOnly, string typeName)
        {
            if (isReadOnly)
                throw new InvalidOperationException($"{typeName} is a Read-Only collection");
        }

        public static void CanOperate(
            [DoesNotReturnIf(false)]bool canOperate,
            [CallerArgumentExpression("canOperate")]string? expression = null,
            string? message = null)
        {
            if (!canOperate)
                throw new InvalidOperationException(message ?? $"The expression '{expression}' is not valid");
        }

        public static void IsInRange<T>(
            [DoesNotReturnIf(false)]bool isInRange,
            T value,
            string? message = null,
            [CallerArgumentExpression("isInRange")]string? expression = null,
            [CallerArgumentExpression("value")]string? valueString = null)
            where T : struct
        {
            if (!isInRange)
                throw new ArgumentOutOfRangeException(valueString, value, message ?? $"The expression {expression} was not satisfied with the values provided.");
        }
    }
}

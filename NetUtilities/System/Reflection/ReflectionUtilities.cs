using System.Runtime.CompilerServices;
#nullable enable
namespace System.Reflection
{
    public static partial class ReflectionUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTuple(Type type)
            => typeof(ITuple).IsAssignableFrom(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetCustomAttribute<T>(Type type, out T attribute, bool inherited = true) where T : Attribute
        {
            attribute = type.GetCustomAttribute<T>(inherited);
            return !(attribute is null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasDefaultConstructor(this Type type)
            => type.IsValueType || !(type.GetConstructor(Type.EmptyTypes) is null);
    }
}

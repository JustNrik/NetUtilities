namespace System.Reflection
{
    public static partial class ReflectionUtilities
    {
        public static bool IsNullable(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static bool TryGetCustomAttribute<T>(Type type, out T attribute, bool inherited = true) where T : Attribute
        {
            attribute = type.GetCustomAttribute<T>(inherited);
            return !(attribute is null);
        }

        public static bool HasDefaultConstructor(this Type t)
            => t.IsValueType || !(t.GetConstructor(Type.EmptyTypes) is null);

        public static readonly Collections.Generic.Dictionary<Type, Delegate> TryParseDelegates
            = new Collections.Generic.Dictionary<Type, Delegate>()
            {
                {typeof(char), char.TryParse }
            };
    }
}

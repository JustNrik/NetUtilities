using System.Runtime.CompilerServices;
using NetUtilities;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Reflection
{
    /// <summary>
    ///     This class provides helper methods for <see cref="Reflection"/>.
    /// </summary>
    public static class ReflectionHelper
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        ///     Indicates if <paramref name="type"/> contains a default constructor. 
        /// </summary>
        /// <remarks>
        ///     Always returns <see langword="true"/> if <paramref name="type"/> is a <see langword="struct"/>.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <see langword="true"/> if the type is a struct or has a parameterless constructor; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool HasDefaultConstructor(this Type type)
            => ConstructorCache.GetOrAddFor(Ensure.NotNull(type));

        /// <summary>
        ///     Indicates if <typeparamref name="T"/> contains a default constructor. 
        /// </summary>
        /// <remarks>
        ///     Always returns <see langword="true"/> if <typeparamref name="T"/> is a <see langword="struct"/>.
        /// </remarks>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <returns>
        ///     <see langword="true"/> if the type is a struct or has a parameterless constructor; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool HasDefaultConstructor<T>()
            => ConstructorCache<T>.Value;

        /// <summary>
        ///     Indicates if <typeparamref name="T"/>'s default value is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <returns>
        ///     <see langword="true"/> if the <typeparamref name="T"/>'s default value is <see langword="null"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool IsNullable<T>()
            => default(T) is null;

        /// <summary>
        ///     Get all types from all assemblies in <see cref="AppDomain.CurrentDomain"/>.
        /// </summary>
        /// <remarks>
        ///     This operation is expensive, so caching the result of this method is recommended.
        /// </remarks>
        /// <returns>
        ///     An array containing all types from all assemblies in <see cref="AppDomain.CurrentDomain"/>.
        /// </returns>
        public static IEnumerable<Type> GetAllTypes()
            => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        private static class ConstructorCache<T>
        {
            public static bool Value = typeof(T).GetConstructor(Type.EmptyTypes) is not null;
        }

        private static class ConstructorCache
        {
            private static readonly Dictionary<Type, bool> _cache = new();

            public static bool GetOrAddFor(Type type)
            {
                if (type.IsValueType)
                    return true;

                if (_cache.TryGetValue(type, out var hasConstructor))
                    return hasConstructor;

                hasConstructor = type.GetConstructor(Type.EmptyTypes) is not null;
                _cache.Add(type, hasConstructor);
                return hasConstructor;
            }
        }
    }
}

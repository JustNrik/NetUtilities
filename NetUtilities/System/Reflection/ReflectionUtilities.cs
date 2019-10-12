using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Reflection
{
    /// <summary>
    /// This class provides utilities for <see cref="Reflection"/>
    /// </summary>
    public static partial class ReflectionUtilities
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        /// Indicates if the type implements <see cref="ITuple"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type">The target type to check</param>
        /// <returns>True if the type implements <see cref="ITuple"/>. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public static bool IsTuple(this Type type)
            => typeof(ITuple).IsAssignableFrom(Ensure.NotNull(type, nameof(type)));

        /// <summary>
        /// Indicates if the type is <see cref="Nullable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type">The target type to check</param>
        /// <returns>True if the type is <see cref="Nullable{T}"/>. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public static bool IsNullable([NotNull]this Type type)
            => Ensure.NotNull(type, nameof(type)).IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Attempts to get the custom attribute of the given type.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <typeparam name="T">The attribute to search.</typeparam>
        /// <param name="type">The type where the attribute will be searched.</param>
        /// <param name="attribute">The output attribute. Null if it can't be found.</param>
        /// <param name="inherited">Checks if it should inspect of ancestors of the given type.</param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool TryGetCustomAttribute<T>(
            [NotNull]this Type type,
            [MaybeNullWhen(false)]out T attribute,
            bool inherited = true) where T : Attribute
        {
            attribute = Ensure.NotNull(type, nameof(type)).GetCustomAttribute<T>(inherited);
            return attribute is object;
        }

        /// <summary>
        /// Returns the specified attribute if exists on the give value of the enum.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enum">The enum.</param>
        /// <param name="inherited">Indicates if it should inspect for the ancestors.</param>
        /// <returns></returns>
        [return: MaybeNull]
        public static TAttribute GetEnumFieldAttribute<TAttribute, TEnum>(this TEnum @enum, bool inherited = true)
            where TAttribute : Attribute
            where TEnum : Enum
            => typeof(TEnum).GetField(Enum.GetName(typeof(TEnum), @enum)).GetCustomAttribute<TAttribute>(inherited);


        /// <summary>
        /// Indicates if the given type contains a default constructor. 
        /// This method will always return true for structs (Structures in Visual Basic)
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <see langword="true"/> if the type is a struct or has a parameterless constructor. 
        /// Otherwise <see langword="false"/>
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool HasDefaultConstructor(this Type type)
            => ConstructorCache.GetOrAddFor(type);


        /// <summary>
        /// Indicates if the given type contains a default constructor. 
        /// This method will always return true for structs (Structures in Visual Basic)
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>
        /// <see langword="true"/> if the type is a struct or has a parameterless constructor. 
        /// Otherwise <see langword="false"/>
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool HasDefaultConstructor<T>()
            => ConstructorCache<T>.Value;

        private static class ConstructorCache<T>
        {
            public static bool Value = typeof(T).IsValueType || typeof(T).GetConstructor(Type.EmptyTypes) is object;
        }

        private static class ConstructorCache
        {
            private static readonly Dictionary<Type, bool> _cache = new Dictionary<Type, bool>();

            public static bool GetOrAddFor(Type type)
            {
                if (type.IsValueType)
                    return true;

                if (_cache.TryGetValue(type, out var hasCtor))
                    return hasCtor;

                _cache.Add(type, type.GetConstructor(Type.EmptyTypes) is object);
                return _cache[type];
            }
        }
    }
}

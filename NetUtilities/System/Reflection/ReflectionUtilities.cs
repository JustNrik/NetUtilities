using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        /// Indicates if the provided type inherits from the base type provided.
        /// </summary>
        /// <param name="derived">The derived type.</param>
        /// <param name="base">The potential base of the type.</param>
        /// <returns></returns>
        public static bool Inherits([NotNull]this Type derived, [NotNull]Type @base)
        {
            if (derived is null)
                Throw.NullArgument(nameof(derived));

            if (@base is null)
                Throw.NullArgument(nameof(@base));

            if (derived == @base)
                return false;

            if (derived.IsInterface)
                return @base.IsInterface && derived.GetTypeInfo().ImplementedInterfaces.Any(i => i == @base);

            return @base.IsAssignableFrom(derived);
        }

        /// <summary>
        /// Indicates if the provided type implements the provided interface type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">The type of the interface to check.</param>
        /// <returns></returns>
        public static bool Implements([NotNull]this Type type, [NotNull]Type interfaceType)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            if (interfaceType is null)
                Throw.NullArgument(nameof(interfaceType));

            if (type.IsInterface || !interfaceType.IsInterface)
                return false;

            return interfaceType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Indicates if the type implements <see cref="ITuple"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type">The target type to check</param>
        /// <returns>True if the type implements <see cref="ITuple"/>. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public static bool IsTuple(this Type type)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            return typeof(ITuple).IsAssignableFrom(type);
        }

        /// <summary>
        /// Indicates if the type is <see cref="Nullable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type">The target type to check</param>
        /// <returns>True if the type is <see cref="Nullable{T}"/>. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public static bool IsNullable([NotNull]this Type type)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            return Nullable.GetUnderlyingType(type) is object;
        }

        /// <summary>
        /// Indicates if the type is <see langword="static"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [MethodImplementation(Inlined)]
        public static bool IsStatic([NotNull]this Type type)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            return type.IsAbstract && type.IsSealed;
        }

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
            if (type is null)
                Throw.NullArgument(nameof(type));

            attribute = type.GetCustomAttribute<T>(inherited);
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
            where TEnum : unmanaged, Enum
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
        public static bool HasDefaultConstructor([NotNull]this Type type)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            return ConstructorCache.GetOrAddFor(type);
        }

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

﻿namespace System.Reflection
{
    using NetUtilities;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using MethodImplementation = Runtime.CompilerServices.MethodImplAttribute;

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
        public static bool IsNullable(this Type type)
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
            this Type type, 
            [DoesNotReturnIf(false)]out T attribute, 
            bool inherited = true) where T : Attribute
        {
            attribute = Ensure.NotNull(type, nameof(type)).GetCustomAttribute<T>(inherited);
            return attribute is object;
        }

        /// <summary>
        /// Indicates if the given type contains a default constructor. 
        /// This method will always return true for structs (Structures in Visual Basic)
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        /// <param name="type"></param>
        /// <returns>True if the type is </returns>
        [MethodImplementation(Inlined)]
        public static bool HasDefaultConstructor(this Type type)
            => Ensure.NotNull(type, nameof(type)).IsValueType 
            || type.GetConstructor(Type.EmptyTypes) is object;
    }
}

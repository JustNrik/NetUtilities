using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NetUtilities;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Reflection
{
    /// <summary>
    ///     This class provides extension methods for <see cref="Reflection"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class ReflectionExtensions
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        /// <summary>
        ///     Indicates if the <paramref name="child"/> type inherits from the <paramref name="parent"/> type.
        /// </summary>
        /// <remarks>
        ///     This method will return <see langword="false"/> if:
        ///     <list type="number">
        ///         <item>
        ///             <paramref name="child"/> is an <see langword="interface"/> but <paramref name="parent"/> is not.
        ///         </item>
        ///         <item>
        ///             <paramref name="child"/> and <paramref name="parent"/> are the same type.
        ///         </item>
        ///         <item>
        ///             <paramref name="child"/> does not inherit from <paramref name="parent"/>.
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <param name="child">
        ///     The child type.
        /// </param>
        /// <param name="parent">
        ///     The parent type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="child"/> inherits from <paramref name="parent"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when either <paramref name="child"/> or <paramref name="parent"/> are <see langword="null"/>.
        /// </exception>
        public static bool Inherits(this Type child, Type parent)
        {
            Ensure.NotNull(child);
            Ensure.NotNull(parent);

            if (child == parent)
                return false;

            if (child.IsInterface)
                return parent.IsInterface && parent.IsAssignableFrom(child);

            return child.IsSubclassOf(parent);
        }

        /// <summary>
        ///     Indicates if the <paramref name="child"/> type implements from the <paramref name="parent"/> type.
        /// </summary>
        /// <remarks>
        ///     This method will return <see langword="false"/> if:
        ///     <list type="number">
        ///         <item>
        ///             <paramref name="child"/> is an <see langword="interface"/>.
        ///         </item>
        ///         <item>
        ///             <paramref name="parent"/> is not an <see langword="interface"/>.
        ///         </item>
        ///         <item>
        ///             <paramref name="child"/> doesn't implement <paramref name="parent"/>.
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <param name="child">
        ///     The child type.
        /// </param>
        /// <param name="parent">
        ///     The parent type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="child"/> implements from <paramref name="parent"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throw when either <paramref name="child"/> or <paramref name="parent"/> are <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool Implements(this Type child, Type parent)
        {
            Ensure.NotNull(child);
            Ensure.NotNull(parent);
            return !child.IsInterface && parent.IsInterface && parent.IsAssignableFrom(child);
        }

        /// <summary>
        ///     Indicates if the type implements <see cref="ITuple"/>.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the type implements <see cref="ITuple"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool IsTuple(this Type type)
            => typeof(ITuple).IsAssignableFrom(Ensure.NotNull(type));

        /// <summary>
        ///     Indicates if the type is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="type">
        ///     The target type to check
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the type is <see cref="Nullable{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool IsNullable(this Type type)
            => Nullable.GetUnderlyingType(Ensure.NotNull(type)) is not null;

        /// <summary>
        ///     Indicates if the type is <see langword="static"/>.
        /// </summary>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="type"/> is <see langword="static"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool IsStatic(this Type type)
        {
            Ensure.NotNull(type);
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        ///     Attempts to get the custom attribute of the given type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the attribute to search.
        /// </typeparam>
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="attribute">
        ///     The attribute.
        /// </param>
        /// <param name="inherited">
        ///     Checks if it should inspect of ancestors of the given type.
        /// </param>
        /// <returns></returns>
        /// <exception cref = "ArgumentNullException"> 
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        [MethodImplementation(Inlined)]
        public static bool TryGetCustomAttribute<T>(this Type type, [NotNullWhen(true)] out T? attribute, bool inherited = true)
            where T : Attribute
            => (attribute = Ensure.NotNull(type).GetCustomAttribute<T>(inherited)) is not null;

        /// <summary>
        ///     Returns the specified attribute if exists on the give value of the <see langword="enum"/>.
        /// </summary>
        /// <typeparam name="TAttribute">
        ///     The type of the attribute.
        /// </typeparam>
        /// <typeparam name="TEnum">
        ///     The type of the <see langword="enum"/>.
        /// </typeparam>
        /// <param name="enum">
        ///     The <see langword="enum"/>.
        /// </param>
        /// <param name="inherited">
        ///     Indicates if it should inspect for the ancestors.
        /// </param>
        /// <returns>
        ///     The specified attribute if exists on the give value of the <see langword="enum"/>; otherwise, <see langword="null"/>.
        /// </returns>
        public static TAttribute? GetEnumFieldAttribute<TAttribute, TEnum>(this TEnum @enum, bool inherited = true)
            where TAttribute : Attribute
            where TEnum : unmanaged, Enum
            => typeof(TEnum).GetRuntimeField(Enum.GetName(@enum) ?? string.Empty)?.GetCustomAttribute<TAttribute>(inherited);

        /// <summary>
        ///     Indicates if <typeparamref name="T"/> is an <see langword="unmanaged"/> type.
        /// </summary>
        /// <remarks>
        ///     This method returns <see langword="true"/> if <typeparamref name="T"/> is an <see langword="unmanaged"/> type.
        ///     An <see langword="unmanaged"/> type meets the following conditions:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 The type is a <see langword="struct"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 None of its fields are reference types, pointers, or structures that constains such fields.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <param name="_">
        ///     This parameter is ignored.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the type is an <see langword="unmanaged"/> type; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImplementation(Inlined)]
        public static bool IsUnmanaged<T>(this T _)
            => !RuntimeHelpers.IsReferenceOrContainsReferences<T>();

        /// <summary>
        ///     Retrieves a collection that represents all methods defined on a specified type.
        /// </summary>
        /// <param name="type">
        ///     The type that contains the methods.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="type"/> is <see langword="null"/>.
        /// </exception>
        /// <returns>
        ///     A collection of methods for the specified type.
        /// </returns>
        public static ConstructorInfo[] GetRuntimeConstructors(this Type type)
            => Ensure.NotNull(type).GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

        /// <inheritdoc cref="Factory{T}.Clone(T)"/>
        [MethodImplementation(Inlined)]
        public static T Clone<T>(this T obj)
            => Factory<T>.Clone(obj);
    }
}
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Reflection
{
    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    public static class Factory
    {
        private static readonly Dictionary<Type, Func<object>> _dict
            = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Gets the instance of a generic type with a parameterless constructor.
        /// Performs much better than <see cref="Activator.CreateInstance(Type)"/>
        /// </summary>
        public static object CreateInstance(Type type)
            => _dict.TryGetValue(type, out var func)
            ? func.Invoke()
            : AddFunc(type);

        private static Func<object> AddFunc(Type type)
        {
            if (!type.HasDefaultConstructor())
                throw new InvalidOperationException($"The type {type.FullName} does not contain a public parameterless constructor.");

            var func = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(type), typeof(object))).Compile();
            _dict.Add(type, func);
            return func;

        }
    }

    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    /// <typeparam name="T">The type whose instance will be created.</typeparam>
    public static class Factory<T> where T : new()
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        private static readonly Func<T> _func = typeof(T).IsValueType
            ? (() => default!)
            : Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        /// <summary>
        /// Creates a single instance (Singleton) which can be used on the application lifetime.
        /// </summary>
        public static T Singleton
        {
            [MethodImplementation(Inlined)]
            [return: NotNull]
            get;
        } = _func.Invoke();

        /// <summary>
        /// Gets the instance of a generic type with a parameterless constructor.
        /// Performs much better than <see cref="Activator.CreateInstance{T}"/>
        /// </summary>
        [return: NotNull]
        public static T CreateInstance()
            => _func.Invoke();
    }
}

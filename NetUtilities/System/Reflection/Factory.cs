﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Reflection
{
    /// <summary>
    ///     This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    public static class Factory
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;

        private static readonly Dictionary<Type, Func<object>> _dict = new();
        private static readonly object _lock = new();

        /// <summary>
        ///     Gets the instance of a generic type with a parameterless constructor.
        ///     Performs much better than <see cref="Activator.CreateInstance(Type)"/>
        /// </summary>
        public static object CreateInstance(Type type)
        {
            lock (_lock)
            {
                if (_dict.TryGetValue(type, out var func))
                    return func();

                return AddFunc(type);
            }
        }

        /// <inheritdoc cref="Factory{T}.CreateInstance"/>
        [MethodImplementation(Inlined)]
        public static T CreateInstance<T>() where T : new()
            => Factory<T>.CreateInstance();

        /// <inheritdoc cref="Factory{T}.Clone(T)"/>
        [MethodImplementation(Inlined)]
        public static T Clone<T>(T obj)
            => Factory<T>.Clone(obj);

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
    ///     This class is a helper to create instance of objects without the performance loss of <see cref="Activator.CreateInstance{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The type whose instance will be created.
    /// </typeparam>
    internal static class Factory<T>
    {
        private static readonly Func<T> _func = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        private static readonly Func<T, T> _clone;
        private static readonly object _lock = new();

        static Factory()
        {
            var method = typeof(T).GetMethod(nameof(MemberwiseClone), BindingFlags.NonPublic | BindingFlags.Instance)!;
            var instance = Expression.Parameter(typeof(T));
            var call = Expression.Call(instance, method);
            var cast = Expression.Convert(call, typeof(T));
            var lambda = Expression.Lambda<Func<T, T>>(cast, instance);

            _clone = lambda.Compile();
        }

        /// <summary>
        ///     Gets a new instance of <typeparamref name="T"/>.
        ///     Performs much better than <see cref="Activator.CreateInstance{T}"/>
        /// </summary>
        public static T CreateInstance()
        {
            lock (_lock)
            {
                if (typeof(T).IsValueType)
                    return default!;

                return _func();
            }
        }

        /// <summary>
        ///     Creates a shallow copy of the provided object.
        /// </summary>
        /// <param name="obj">
        ///     The object to be cloned.
        /// </param>
        /// <returns>
        ///     A shallow copy of the provided object.
        /// </returns>
        public static T Clone(T obj)
        {
            lock (_lock)
                return _clone(obj);
        }
    }
}

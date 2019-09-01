namespace System.Reflection
{
    using System.Linq.Expressions;

    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    /// <typeparam name="T">The type whose instance will be created.</typeparam>
    public static class Factory<T> where T : notnull, new()
    {

        private static readonly Func<T> _func = typeof(T).IsValueType
            ? (() => default!)
            : Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        /// <summary>
        /// Gets the instance of a generic type with a parameterless constructor.
        /// Performs much better than <see cref="Activator.CreateInstance{T}"/>
        /// </summary>
        public static T CreateInstance()
            => _func.Invoke();

        /// <summary>
        /// Creates a single instance (Singleton) which can be used on the application lifetime.
        /// </summary>
        public static T Singleton { get; } = _func.Invoke();
    }

    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    public static class Factory<TIn, TOut>
    {
        /// <summary>
        /// Creates an instance of <typeparamref name="TOut"/> with the given argument of type <typeparamref name="TIn"/>.
        /// </summary>
        /// <typeparam name="TIn">The type of the argument.</typeparam>
        /// <typeparam name="TOut">The type of the object that will be created.</typeparam>
        /// <param name="in">The argument.</param>
        /// <returns>An instance of <typeparamref name="TOut"/></returns>
        public static TOut CreateInstance(TIn @in) => _func.Invoke(@in);
        private static readonly Func<TIn, TOut> _func;

        static Factory()
        {
            var ctor = typeof(TOut).GetConstructor(new Type[] { typeof(TIn) });
            var parameter = Expression.Parameter(typeof(TIn), "@in");
            var @new = Expression.New(ctor, parameter);
            var lambda = Expression.Lambda<Func<TIn, TOut>>(@new, parameter);
            _func = lambda.Compile();
        }
    }
}

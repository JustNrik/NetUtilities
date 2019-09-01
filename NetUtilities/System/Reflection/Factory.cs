namespace System.Reflection
{
    using System.Linq.Expressions;

    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    /// <typeparam name="T">The type whose instance will be created.</typeparam>
    public static class Factory<T> where T : notnull, new()
    {
        /// <summary>
        /// Gets the instance of a generic type with a parameterless constructor.
        /// Performs much better than <see cref="Activator.CreateInstance{T}"/>
        /// </summary>
        public static T CreateInstance()
            => _instanceDelegate.Invoke();

        /// <summary>
        /// Creates a <see cref="Scope{T}"/> that contains an object.
        /// </summary>
        /// <returns><see cref="Scope{T}"/></returns>
        public static Scope<T> CreateScope()
            => new Scope<T>(_instanceDelegate.Invoke());

        /// <summary>
        /// Creates a single instance (Singleton) which can be used on the application lifetime.
        /// </summary>
        public static T Singleton { get; } = _instanceDelegate.Invoke();

        private static readonly Func<T> _instanceDelegate = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    }

    public static class Factory<TIn, TOut>
    {
        private const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public static TOut CreateInstance(TIn @in) => _func.Invoke(@in);
        private static readonly Func<TIn, TOut> _func =
            Expression.Lambda<Func<TIn, TOut>>(
                Expression.New(
                    typeof(TOut)
                    .GetConstructor(AllFlags, null, new Type[] { typeof(TIn) }, null)),
                Expression.Parameter(typeof(TIn), "@in"))
            .Compile();
    }
}

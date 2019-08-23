namespace System.Reflection
{
    using System.Linq.Expressions;

    /// <summary>
    /// This class is a helper to create instance of objects whose types are only known at runtime.
    /// </summary>
    /// <typeparam name="T">The type whose instance will be created.</typeparam>
    public static class Instantiator<T> where T : new()
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
        /// <returns></returns>
        public static Scope<T> CreateScopedInstance()
            => new Scope<T>(CreateInstance());

        /// <summary>
        /// Creates a single instance (Singleton) which can be used on the application lifetime.
        /// </summary>
        public static T SingleInstance { get; } = _instanceDelegate.Invoke();

        private static readonly Func<T> _instanceDelegate = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    }
}

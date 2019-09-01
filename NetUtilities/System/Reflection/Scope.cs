namespace System.Reflection
{
    using NetUtilities;

    /// <summary>
    /// Represents a generic Scope for service providing scoped instances.
    /// </summary>
    /// <typeparam name="T">The type of the object</typeparam>
    public readonly ref struct Scope<T> where T : notnull, new()
    {
        /// <summary>
        /// An instance of <typeparamref name="T"/>
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Creates an instance of <see cref="Scope{T}"/> providing a non-null object/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when instance is null</exception>
        /// <param name="instance"></param>
        public Scope(T instance)
            => Instance = Ensure.NotNull(instance, nameof(instance));
    }
}

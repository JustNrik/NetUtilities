namespace System
{
    /// <summary>
    ///     Provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public interface ILazy<out T>
    {
        /// <summary>
        ///     Gets the value.
        /// </summary>
        T Value { get; }

        /// <summary>
        ///     Indicates if the value is initialized.
        /// </summary>
        bool IsInitialized { get; }
    }
}

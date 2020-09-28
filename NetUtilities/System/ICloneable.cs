namespace System
{
    /// <summary>
    ///     Supports cloning, which creates a new instance of a class with the same value as an existing instance.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public interface ICloneable<T> 
    {
        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        T Clone();
    }
}

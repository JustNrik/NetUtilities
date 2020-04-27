namespace System
{
    /// <inheritdoc cref="ICloneable"/>
    public interface ICloneable<T> 
    {
        /// <inheritdoc cref="ICloneable.Clone"/>
        T Clone();
    }
}

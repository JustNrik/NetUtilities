namespace System.Collections.Generic
{
    public interface IEquatable64<T>
    {
        bool Equals(T other);
        long GetHashCode64();
    }
}
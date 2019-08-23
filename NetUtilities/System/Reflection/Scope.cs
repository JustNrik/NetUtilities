
namespace System.Reflection
{
    public readonly ref struct Scope<T> where T : new()
    {
        public T Instance { get; }

        public Scope(T instance)
            => Instance = instance;
    }
}

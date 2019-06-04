using System.Linq.Expressions;
using System.Reflection.Emit;
#nullable enable
namespace System.Reflection
{
    public static class Instantiator<T>
    {
        /// <summary>
        /// Gets the instance of a generic type with a parameterless constructor. Performs much better than <see cref="Activator.CreateInstance{T}"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when a parameterless constructor is not found.</exception>
        public static T GetInstance()
            => _instanceDelegate();

        private static readonly Func<T> _instanceDelegate = CreateInstance();

        private static Func<T> CreateInstance()
        {
            var type = typeof(T);
            if (type == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (type.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();

            throw new InvalidOperationException("No parameterless constructor found.");
        }
    }
}

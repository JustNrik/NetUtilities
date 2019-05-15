using System.Linq.Expressions;
#nullable enable
namespace System.Reflection
{
    public static class Instantiator<T>
    {
        /// <summary>
        /// Get's the instance of a generic type with a parameterless ctor. Performs much better than Activator.CreateInstance()
        /// </summary>
        public static readonly Func<T> GetInstance = CreateInstance();

        private static Func<T> CreateInstance()
        {
            var t = typeof(T);
            if (t == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (t.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

            throw new Exception("No parameterless constructor found.");
        }
    }
}

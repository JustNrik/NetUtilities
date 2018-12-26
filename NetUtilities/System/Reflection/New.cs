using System.Linq.Expressions;

namespace System.Reflection
{
    public static class New<T>
    {
        public static readonly Func<T> Instance = CreateInstance();

        static Func<T> CreateInstance()
        {
            var t = typeof(T);
            if (t == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (t.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(t)).Compile();

            throw new Exception("No paramerteless ctro found.");
        }
    }
}

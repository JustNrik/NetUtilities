using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
#nullable enable
namespace System
{
    public readonly ref struct BindedScope
    {
        private readonly Action _undoAction;
        public BindedScope(Action undoFunc) 
            => _undoAction = undoFunc;
        private static Action CreateAction<TResult>(object? obj, MemberInfo? memberInfo, TResult newValue)
        {
            TResult oldValue;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    oldValue = (TResult)propertyInfo.GetMethod.Invoke(obj, Array.Empty<object>());
                    propertyInfo.SetMethod.Invoke(obj, new object[] { newValue! });
                    return () => propertyInfo.SetMethod.Invoke(obj, new object[] { oldValue });
                case FieldInfo fieldInfo:
                    oldValue = (TResult)fieldInfo.GetValue(obj);
                    fieldInfo.SetValue(obj, newValue);
                    return () => fieldInfo.SetValue(obj, oldValue);
                default:
                    throw new InvalidOperationException("The argument memberInfo must be a Property or a Field");
            }
        }

        public static BindedScope Create<TValue>(Expression<Func<TValue>> propertyFunc, TValue newValue)
        {
            var memberInfo = (propertyFunc.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(null, memberInfo, newValue);
            return new BindedScope(undoFunc);
        }

        public static BindedScope Create<T, TValue>(T obj, Expression<Func<T, TValue>> propertyFunc, TValue newValue)
        {
            var memberInfo = (propertyFunc.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(obj, memberInfo, newValue);
            return new BindedScope(undoFunc);
        }

        public void Dispose()
            => _undoAction();

        public ValueTask DisposeAsync()
        {
            _undoAction();
            return new ValueTask();
        }
    }
}
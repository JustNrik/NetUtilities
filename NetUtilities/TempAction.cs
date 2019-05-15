using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace System
{
    public readonly ref struct TempAction
    {
        private readonly Action _undoAction;
        public TempAction(Action undoFunc) 
            => _undoAction = undoFunc;
        private static Action CreateAction<TResult>(object obj, MemberInfo memberInfo, TResult newValue)
        {
            TResult oldValue;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    oldValue = (TResult)propertyInfo.GetMethod.Invoke(obj, Array.Empty<object>());
                    propertyInfo.SetMethod.Invoke(obj, new object[] { newValue });
                    return () => propertyInfo.SetMethod.Invoke(obj, new object[] { oldValue });
                case FieldInfo fieldInfo:
                    oldValue = (TResult)fieldInfo.GetValue(obj);
                    fieldInfo.SetValue(obj, newValue);
                    return () => fieldInfo.SetValue(obj, oldValue);
                default:
                    throw new InvalidOperationException("The argument memberInfo must be a Property or a Field");
            }
        }

        public static TempAction CreateScope<TResult>(Expression<Func<TResult>> propertyFunc, TResult newValue)
        {
            var memberInfo = (propertyFunc.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(null, memberInfo, newValue);
            return new TempAction(undoFunc);
        }

        public static TempAction CreateScope<T, TResult>(T obj, Expression<Func<T, TResult>> propertyFunc, TResult newValue)
        {
            var memberInfo = (propertyFunc.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(obj, memberInfo, newValue);
            return new TempAction(undoFunc);
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
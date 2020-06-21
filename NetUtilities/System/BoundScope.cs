using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace System
{
    public readonly ref struct BoundScope
    {
        private readonly Action _undoAction;
        private readonly CancellationTokenSource _cts;

        private BoundScope(Action undoAction)
        {
            _undoAction = undoAction;
            _cts = new CancellationTokenSource();
        }

        private static Action CreateAction<T, TResult>([AllowNull]T obj, MemberInfo? memberInfo, TResult newValue)
        {
            TResult oldValue;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    if (propertyInfo.GetMethod is null || propertyInfo.SetMethod is null)
                        throw new InvalidOperationException($"The property {propertyInfo.Name} must have getter and setter.");

                    if (obj is not null)
                    {
                        var property = Mapper<T>.Properties.Find(x => x.Member == propertyInfo);

                        oldValue = (TResult)property.GetValue(obj);
                        property.SetValue(obj, newValue);
                        return () => property.SetValue(obj, oldValue);
                    }
                    
                    oldValue = (TResult)propertyInfo.GetMethod.Invoke(obj, Array.Empty<object>());
                    propertyInfo.SetMethod.Invoke(obj, new object[] { newValue! });
                    return () => propertyInfo.SetMethod.Invoke(obj, new object?[] { oldValue });
                case FieldInfo fieldInfo:
                    if (obj is not null)
                    {
                        var field = Mapper<T>.Fields.Find(x => x.Member == fieldInfo);

                        oldValue = (TResult)field.GetValue(obj);
                        field.SetValue(obj, newValue);
                        return () => field.SetValue(obj, oldValue);
                    }

                    oldValue = (TResult)fieldInfo.GetValue(obj);
                    fieldInfo.SetValue(obj, newValue);
                    return () => fieldInfo.SetValue(obj, oldValue);
                default:
                    throw new InvalidOperationException("The argument memberInfo must be a Property or a Field");
            }
        }

        public static BoundScope Create<TValue>(Expression<Func<TValue>> expression, TValue newValue)
        {
            var memberInfo = (expression.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(default(object), memberInfo, newValue);
            return new BoundScope(undoFunc);
        }

        public static BoundScope Create<T, TValue>(T obj, Expression<Func<T, TValue>> expression, TValue newValue)
        {
            var memberInfo = (expression.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(obj, memberInfo, newValue);
            return new BoundScope(undoFunc);
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction.Invoke();

            _cts.Dispose();
        }
    }
}
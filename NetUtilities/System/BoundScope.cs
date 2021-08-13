using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

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

        private static Action CreateAction<T, TResult>([AllowNull] T obj, MemberInfo? memberInfo, TResult newValue)
        {
            TResult oldValue;

            switch (memberInfo)
            {
                case PropertyInfo propertyInfo:
                    if (propertyInfo.GetMethod is null || propertyInfo.SetMethod is null)
                        throw new InvalidOperationException($"The property {propertyInfo.Name} must have getter and setter.");

                    if (obj is not null)
                    {
                        oldValue = (TResult)propertyInfo.GetValue(obj)!;
                        propertyInfo.SetValue(obj, newValue);
                        return () => propertyInfo.SetValue(obj, oldValue);
                    }

                    oldValue = (TResult)propertyInfo.GetMethod.Invoke(obj, Array.Empty<object>())!;
                    propertyInfo.SetMethod.Invoke(obj, new object[] { newValue! });
                    return () => propertyInfo.SetMethod.Invoke(obj, new object?[] { oldValue });
                case FieldInfo fieldInfo:
                    if (obj is not null)
                    {
                        oldValue = (TResult)fieldInfo.GetValue(obj)!;
                        fieldInfo.SetValue(obj, newValue);
                        return () => fieldInfo.SetValue(obj, oldValue);
                    }

                    oldValue = (TResult)fieldInfo.GetValue(obj)!;
                    fieldInfo.SetValue(obj, newValue);
                    return () => fieldInfo.SetValue(obj, oldValue);
                default:
                    throw new InvalidOperationException("The argument memberInfo must be a Property or a Field");
            }
        }

        /// <summary>
        ///     Creates a <see cref="BoundScope"/> with the provided value and expression.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The target value's type.
        /// </typeparam>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="newValue">
        ///     The value.
        /// </param>
        /// <returns>
        ///     A <see cref="BoundScope"/> with the provided value and expression.
        /// </returns>
        public static BoundScope Create<TValue>(Expression<Func<TValue>> expression, TValue newValue)
        {
            var memberInfo = (expression.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(default(object), memberInfo, newValue);
            return new BoundScope(undoFunc);
        }

        /// <summary>
        ///     Creates a <see cref="BoundScope"/> with the provided objects and expression.
        /// </summary>
        /// <typeparam name="T">
        ///     The object's type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The target value's type.
        /// </typeparam>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="newValue">
        ///     The value.
        /// </param>
        /// <returns>
        ///     A <see cref="BoundScope"/> with the provided objects and expression.
        /// </returns>
        public static BoundScope Create<T, TValue>(T obj, Expression<Func<T, TValue>> expression, TValue newValue)
        {
            var memberInfo = (expression.Body as MemberExpression)?.Member;
            var undoFunc = CreateAction(obj, memberInfo, newValue);
            return new BoundScope(undoFunc);
        }

        /// <summary>
        ///     Prevents the undo action to be executed.
        /// </summary>
        public void Cancel()
            => _cts.Cancel();

        /// <summary>
        ///     Executes the undo action and disposes the internal <see cref="CancellationTokenSource"/>.
        /// </summary>
        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction.Invoke();

            _cts.Dispose();
        }
    }
}
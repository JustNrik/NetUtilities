using System;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    public class FieldData : MemberData<FieldInfo>
    {
        private readonly Func<object?, object?> _get;
        private readonly Action<object?, object?> _set;

        public Type FieldType { get; }
        public bool IsNullable { get; }
        public bool IsStatic { get; }

        public FieldData(FieldInfo field) : base(field)
        {
            FieldType = field.FieldType;
            IsNullable = FieldType.IsClass || FieldType.IsInterface || FieldType.IsNullable();
            IsStatic = field.IsStatic;

            var parameter = Expression.Parameter(typeof(object));
            var cast = Expression.Convert(parameter, FieldType.DeclaringType!); // will be null if it's a module property, cba to handle it
            var prop = Expression.Field(cast, FieldType.Name);
            var convert = Expression.Convert(prop, typeof(object));
            var lambda = Expression.Lambda<Func<object?, object?>>(convert, parameter);

            _get = lambda.Compile();

            var instance = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));
            var convertInstance = Expression.Convert(instance, FieldType.DeclaringType!);
            var convertValue = Expression.Convert(value, FieldType);
            var prop2 = Expression.Field(convertInstance, FieldType.Name);
            var assign = Expression.Assign(prop2, convertValue);
            var lambda2 = Expression.Lambda<Action<object?, object?>>(assign, instance, value);

            _set = lambda2.Compile();
        }

        public object? GetValue(object? target)
        {
            Ensure.CanOperate(
                target is not null ^ IsStatic,
                $"The target cannot be null because {Member.DeclaringType}.{Member.Name} is not an static property.");

            return _get(target);
        }

        public void SetValue(object? target, object? value)
        {
            Ensure.CanOperate(
                target is not null ^ IsStatic,
                $"The target cannot be null because {Member.DeclaringType}.{Member.Name} is not an static property.");
            Ensure.CanOperate(
                value is not null || IsNullable,
                $"The value cannot be null because {Member.DeclaringType}.{Member.Name} is of type {FieldType.Name}, which is not a nullable type (class, interface or Nullable<{FieldType.Name}>)");

            _set(target, value);
        }
    }
}

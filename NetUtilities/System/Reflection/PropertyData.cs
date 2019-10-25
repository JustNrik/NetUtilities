using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public class PropertyData : MemberData<PropertyInfo>
    {
        private readonly Func<object?, object?>? _get;
        private readonly Action<object?, object?>? _set;

        public ReadOnlyList<ParameterInfo> Parameters { get; }
        public MethodInfo? Getter { get; }
        public MethodInfo? Setter { get; }
        public Type PropertyType { get; }
        public bool IsNullable { get; }
        public bool IsIndexer { get; }
        public bool IsStatic { get; }


        public PropertyData(PropertyInfo property) : base(property)
        {
            PropertyType = property.PropertyType;
            Parameters = property.GetIndexParameters().ToReadOnlyList();
            Getter = property.GetGetMethod() ?? property.GetGetMethod(true);
            Setter = property.GetSetMethod() ?? property.GetSetMethod(true);
            IsIndexer = Parameters.Count > 0;
            IsNullable = PropertyType.IsClass || PropertyType.IsInterface || PropertyType.IsNullable();
            IsStatic = PropertyType.IsStatic();

            if (Getter is object)
            {
                var parameter = Expression.Parameter(property.DeclaringType, "instance");
                var getter = Expression.Property(parameter, property.Name);
                var cast = Expression.Convert(getter, typeof(object));
                var lambda = Expression.Lambda(cast, parameter);

                Debug.Assert(lambda.ToString() == $"instance => Convert(instance.{Member.Name}, Object)");

                _get = Unsafe.As<Func<object?, object?>>(lambda.Compile());
            }

            if (Setter is object)
            {
                var parameter = Expression.Parameter(property.DeclaringType, "instance"); 
                var propParam = Expression.Parameter(typeof(object), "value");            
                var param = Expression.Convert(propParam, property.PropertyType);        
                var setter = Expression.Property(parameter, property.Name);               
                var assign = Expression.Assign(setter, param);                            
                var lambda = Expression.Lambda(assign, parameter, propParam);             

                Debug.Assert(lambda.ToString() == $"(instance, value) => instance.{Member.Name} = Convert(value, {Member.PropertyType})");

                _set = Unsafe.As<Action<object?, object?>>(lambda.Compile());
            }
        }

        public void SetValue(object? target, object? value)
        {
            if (_set is null)
                Throw.InvalidOperation($"The property {Member.DeclaringType}.{Member.Name} doesn't contain a setter.");

            if (target is null && !IsStatic)
                Throw.InvalidOperation($"The target cannot be null because {Member.DeclaringType}.{Member.Name} is not an static property.");

            if (value is null && !IsNullable)
                Throw.InvalidOperation($"The value cannot be null because {Member.DeclaringType}.{Member.Name} is of type {PropertyType}, which is not a nullable type (class, interface or Nullable<T>)");

            _set(target, value);
        }

        public object? GetValue(object? target)
        {
            if (_get is null)
                Throw.InvalidOperation($"The property {Member.DeclaringType}.{Member.Name} doesn't contain a getter.");

            if (target is null && !IsStatic)
                Throw.InvalidOperation($"The target cannot be null because {Member.DeclaringType}.{Member.Name} is not an static property.");

            return _get(target);
        }
    }
}

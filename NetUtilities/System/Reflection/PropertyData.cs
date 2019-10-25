using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
    public class PropertyData : MemberData
    {
        private readonly Func<object, object>? _get;
        private readonly Action<object, object>? _set;

        public new PropertyInfo Member => (PropertyInfo)base.Member;
        public ReadOnlyList<ParameterInfo> Parameters { get; }
        public MethodInfo? Getter { get; }
        public MethodInfo? Setter { get; }
        public bool IsIndexer => Parameters.Count > 0;

        public PropertyData(PropertyInfo property) : base(property)
        {
            Parameters = property.GetIndexParameters().ToReadOnlyList();
            Getter = property.GetGetMethod() ?? property.GetGetMethod(true);
            Setter = property.GetSetMethod() ?? property.GetSetMethod(true);

            if (Getter is object)
            {
                var parameter = Expression.Parameter(property.DeclaringType, "instance");
                var getter = Expression.Property(parameter, property.Name);
                var cast = Expression.Convert(getter, typeof(object));
                var lambda = Expression.Lambda(cast, parameter);

                Debug.Assert(lambda.ToString() == $"instance => Convert(instance.{Member.Name}, Object)");

                _get = Unsafe.As<Func<object, object>>(lambda.Compile());
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

                _set = Unsafe.As<Action<object, object>>(lambda.Compile());
            }
        }

        public void SetValue(object target, object value)
        {
            if (_set is null)
                Throw.InvalidOperation($"The property {Member.DeclaringType}.{Member.Name} doesn't contain a setter.");

            _set(target, value);
        }

        public object GetValue(object target)
        {
            if (_get is null)
                Throw.InvalidOperation($"The property {Member.DeclaringType}.{Member.Name} doesn't contain a getter.");

            return _get(target);
        }
    }
}

using NetUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
                var parameter = Expression.Parameter(typeof(object), "instance");
                var cast = Expression.Convert(parameter, property.DeclaringType);
                var prop = Expression.Property(cast, property.Name);
                var convert = Expression.Convert(prop, typeof(object));
                var lambda = Expression.Lambda<Func<object?, object?>>(convert, parameter);

                _get = lambda.Compile();
            }

            if (Setter is object)
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var value = Expression.Parameter(typeof(object), "value");
                var convertInstance = Expression.Convert(instance, property.DeclaringType);
                var convertValue = Expression.Convert(value, property.PropertyType);
                var prop = Expression.Property(convertInstance, property.Name);
                var assign = Expression.Assign(prop, convertValue);
                var lambda = Expression.Lambda<Action<object?, object?>>(assign, instance, value);

                _set = lambda.Compile();
            }
        }

        /// <summary>
        /// Sets the value of this property given the instance and the value. If the property is <see langword="static"/>, pass <see langword="null"/> on the target.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the property has no setter, the target is <see langword="null"/> and the property is not <see langword="static"/> or the value is <see langword="null"/> and the property is not nullable.</exception>
        /// <param name="target">The target instance. Pass <see langword="null"/> if the property is <see langword="static"/>.</param>
        /// <param name="value">The value.</param>
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

        /// <summary>
        /// Gets the value of this property given the instance. If the property is <see langword="static"/>, pass <see langword="null"/> on the target.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the property has no getter or the target is <see langword="null"/> and the property is not <see langword="static"/></exception>
        /// <param name="target">The target instance. Pass <see langword="null"/> if the property is <see langword="static"/>.</param>
        /// <returns>The value of the property.</returns>
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

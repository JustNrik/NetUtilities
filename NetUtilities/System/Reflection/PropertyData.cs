using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class PropertyData : MemberData<PropertyInfo>
    {
        private readonly Lazy<Func<object?, object?>>? _get;
        private readonly Lazy<Action<object?, object?>>? _set;

        /// <summary>
        ///     Gets the parameters of the property this data reflects.
        /// </summary>
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        /// <summary>
        ///     Gets the <see cref="MethodInfo"/> that reflects the getter of this data reflected property.
        /// </summary>
        public MethodInfo? Getter { get; }

        /// <summary>
        ///     Gets the <see cref="MethodInfo"/> that reflects the setter of this data reflected property.
        /// </summary>
        public MethodInfo? Setter { get; }

        /// <summary>
        ///     Gets the <see cref="Type"/> of the property this data reflects.
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        ///     Indicates if the property this data reflects returns a nullable type (class, interface or <see cref="Nullable{T}"/>).
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        ///     Indicates if the property this data reflects is an indexer.
        /// </summary>
        public bool IsIndexer { get; }

        /// <summary>
        ///     Indicates if the property this data reflects is <see langword="static"/>.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="PropertyData"/> class 
        ///     with the provided <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        public PropertyData(PropertyInfo property) : base(property)
        {
            PropertyType = property.PropertyType;
            Parameters = property.GetIndexParameters().ToReadOnlyList();
            Getter = property.GetGetMethod(false) ?? property.GetGetMethod(true);
            Setter = property.GetSetMethod(false) ?? property.GetSetMethod(true);
            IsIndexer = Parameters.Count > 0;
            IsNullable = PropertyType.IsClass || PropertyType.IsInterface || PropertyType.IsNullable();
            IsStatic = PropertyType.IsAbstract && PropertyType.IsSealed;

            if (Getter is not null)
            {
                _get = new(() =>
                {
                    var parameter = Expression.Parameter(typeof(object));
                    var cast = Expression.Convert(parameter, property.DeclaringType!); // will be null if it's a module property, cba to handle it
                    var prop = Expression.Property(cast, property.Name);
                    var convert = Expression.Convert(prop, typeof(object));
                    var lambda = Expression.Lambda<Func<object?, object?>>(convert, parameter);
                    return lambda.Compile();
                }, true);
            }

            if (Setter is not null)
            {
                _set = new(() =>
                {
                    var instance = Expression.Parameter(typeof(object));
                    var value = Expression.Parameter(typeof(object));
                    var convertInstance = Expression.Convert(instance, property.DeclaringType!);
                    var convertValue = Expression.Convert(value, property.PropertyType);
                    var prop = Expression.Property(convertInstance, property.Name);
                    var assign = Expression.Assign(prop, convertValue);
                    var lambda = Expression.Lambda<Action<object?, object?>>(assign, instance, value);
                    return lambda.Compile();
                });
            }
        }

        /// <summary>
        ///     Gets the value of the property this data reflects given the instance. 
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the property is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must <b>not</b> be <see langword="null"/> if the property is <b>not</b> <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the property has no getter. 
        ///     -- or --
        ///     when the instance is <see langword="null"/> but the property is not <see langword="static"/>.
        ///     -- or --
        ///     when the instance is not <see langword="null"/> but the property is <see langword="static"/>.
        /// </exception>
        /// <returns>
        ///     The value of the property this data reflects.
        /// </returns>
        public object? GetValue(object? instance)
        {
            if (_get is null)
                throw new InvalidOperationException(
                    $"The property {Member.DeclaringType}.{Member.Name} doesn't have a getter.");

            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static field.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static field.");

            return _get.Value(instance);
        }

        /// <summary>
        ///     Gets the value of the property this data reflects given the instance. 
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the property is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must <b>not</b> be <see langword="null"/> if the property is <b>not</b> <see langword="static"/>.<br/>
        ///     The <paramref name="value"/> must <b>not</b> be <see langword="null"/> if the property type is <b>not</b> a nullable type (<see langword="class"/>, <see langword="interface"/> or <see cref="Nullable{T}"/>).
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the property has no getter. 
        ///     -- or --
        ///     when the instance is <see langword="null"/> but the property is not <see langword="static"/>.
        ///     -- or --
        ///     when the instance is not <see langword="null"/> but the property is <see langword="static"/>.
        /// </exception>
        public void SetValue(object? instance, object? value)
        {
            if (_set is null)
                throw new InvalidOperationException(
                    $"The property {Member.DeclaringType}.{Member.Name} doesn't have a getter.");

            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static field.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static field.");

            if (value is null && !IsNullable)
                throw new InvalidOperationException(
                    $"The value cannot be null because {Member.DeclaringType}.{Member.Name} is of type {PropertyType.Name}, which is not a nullable type (class, interface or Nullable<{PropertyType.Name}>)");

            _set.Value(instance, value);
        }
    }
}

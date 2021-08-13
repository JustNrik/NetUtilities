using System.Linq.Expressions;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class PropertyData : MemberData<PropertyInfo>
    {
        private readonly ConcurrentLazy<Func<object?, object?[]?, object?>, (ParameterInfo[], MethodInfo)>? _get;
        private readonly ConcurrentLazy<Action<object?, object?[]?>, MethodInfo>? _set;

        /// <summary>
        ///     Gets the parameters of the property this data reflects.
        /// </summary>
        public ReadOnlyList<ParameterInfo> IndexParameters { get; init; }

        /// <summary>
        ///     Gets the <see cref="MethodInfo"/> that reflects the getter of this data reflected property.
        /// </summary>
        public MethodInfo? Getter { get; init; }

        /// <summary>
        ///     Gets the <see cref="MethodInfo"/> that reflects the setter of this data reflected property.
        /// </summary>
        public MethodInfo? Setter { get; init; }

        /// <summary>
        ///     Gets the <see cref="Type"/> of the property this data reflects.
        /// </summary>
        public Type PropertyType { get; init; }

        /// <summary>
        ///     Indicates if the property this data reflects returns a nullable type
        ///     (<see langword="class"/>, <see langword="interface"/> or <see cref="Nullable{T}"/>).
        /// </summary>
        public bool IsNullable { get; init; }

        /// <summary>
        ///     Indicates if the property this data reflects is an indexer.
        /// </summary>
        public bool IsIndexer { get; init; }

        /// <summary>
        ///     Indicates if the property this data reflects is <see langword="static"/>.
        /// </summary>
        public bool IsStatic { get; init; }

        /// <summary>
        ///     Initializes a new instance of <see cref="PropertyData"/> class 
        ///     with the provided <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        public PropertyData(PropertyInfo property) : base(property)
        {
            var parameters = property.GetIndexParameters();
            PropertyType = property.PropertyType;
            IndexParameters = parameters.ToReadOnlyList();
            Getter = property.GetGetMethod(true);
            Setter = property.GetSetMethod(true);
            IsIndexer = IndexParameters.Count > 0;
            IsNullable = PropertyType.IsClass || PropertyType.IsInterface || PropertyType.IsNullable();
            IsStatic = PropertyType.IsStatic();

            if (Getter is not null)
            {
                _get = new(static args =>
                {
                    var (parameters, method) = args;
                    var instance = Expression.Parameter(typeof(object));
                    var array = Expression.Parameter(typeof(object[]));
                    var indexes = parameters
                        .Select((arg, index) => Expression.Convert(
                            Expression.ArrayIndex(array, Expression.Constant(index)),
                            arg.ParameterType))
                        .ToArray();
                    var call = Expression.Call(
                        method.IsStatic ? null : Expression.Convert(instance, method.DeclaringType!),
                        method,
                        indexes.Length == 0 ? null : indexes);
                    var convert = Expression.Convert(call, typeof(object));
                    return Expression.Lambda<Func<object?, object?[]?, object?>>(convert, array).Compile();
                }, (parameters, Getter));
            }

            if (Setter is not null)
            {
                _set = new(static setter =>
                {
                    var parameters = setter.GetParameters();
                    var instance = Expression.Parameter(typeof(object));
                    var array = Expression.Parameter(typeof(object[]));
                    var indexes = parameters.Select((arg, index) => Expression.Convert(
                        Expression.ArrayIndex(array, Expression.Constant(index)),
                        arg.ParameterType));
                    var call = Expression.Call(
                        setter.IsStatic ? null : Expression.Convert(instance, setter.DeclaringType!),
                        setter,
                        indexes);
                    return Expression.Lambda<Action<object?, object?[]?>>(call, array).Compile();
                }, Setter);
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
        public object? GetValue(object? instance, params object?[]? parameters)
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

            return _get.Value(instance, parameters);
        }

        /// <summary>
        ///     Sets the value of the property this data reflects given the instance. 
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
        ///     Thrown when the property has no setter. 
        ///     -- or --
        ///     when the instance is <see langword="null"/> but the property is not <see langword="static"/>.
        ///     -- or --
        ///     when the instance is not <see langword="null"/> but the property is <see langword="static"/>.
        /// </exception>
        public void SetValue(object? instance, params object?[] parameters)
        {
            if (_set is null)
                throw new InvalidOperationException(
                    $"The property {Member.DeclaringType}.{Member.Name} doesn't have a setter.");

            if (parameters is null or { Length: 0 })
                throw new InvalidOperationException("You must provide at least one parameter.");

            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static field.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static field.");

            if (parameters.Length == 1 && parameters[0] is null && !IsNullable)
                throw new InvalidOperationException(
                    $"The value cannot be null because {Member.DeclaringType}.{Member.Name} is of type {PropertyType.Name}, which is not a nullable type (class, interface or Nullable<{PropertyType.Name}>)");

            _set.Value(instance, parameters);
        }
    }
}

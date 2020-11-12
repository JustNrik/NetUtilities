using System;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class FieldData : MemberData<FieldInfo>
    {
        private readonly Func<object?, object?> _get;
        private readonly Action<object?, object?> _set;

        /// <summary>
        ///     Gets the type of the field this data reflects.
        /// </summary>
        public Type FieldType { get; init; }

        /// <summary>
        ///     Indicates if the type of the field this data reflects is a nullable type 
        ///     (<see langword="class"/>, <see langword="interface"/> or <see cref="Nullable{T}"/>).
        /// </summary>
        public bool IsNullable { get; init; }

        /// <summary>
        ///     Indicates if the field this data reflects is <see langword="static"/>.
        /// </summary>
        public bool IsStatic { get; init; }

        /// <summary>
        ///     Initializes a new instance of <see cref="FieldData"/> class 
        ///     with the provided <see cref="FieldInfo"/> and target.
        /// </summary>
        /// <param name="field">
        ///     The field.
        /// </param>
        public FieldData(FieldInfo field) : base(field)
        {
            Ensure.NotNull(field);

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

        /// <summary>
        ///     Gets the value of the field reflected in the instance provided.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the field is <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="instance"/> is <see langword="null"/> but the field is an instance field. 
        ///     -- or --
        ///     when <paramref name="instance"/> is not <see langword="null"/> but the field is a <see langword="static"/> field. 
        /// </exception>
        /// <returns>
        ///     The value of the field reflected in the instance provided.
        /// </returns>
        public object? GetValue(object? instance)
        {
            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static field.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static field.");

            return _get(instance);
        }

        /// <summary>
        ///     Gets the value of the field reflected in the instance and value provided.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the field is <see langword="static"/>.<br/>
        ///     The <paramref name="value"/> must <b>not</b> be <see langword="null"/> if the property type is <b>not</b> a nullable type (<see langword="class"/>, <see langword="interface"/> or <see cref="Nullable{T}"/>).
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="instance"/> is <see langword="null"/> but the field is an instance field. 
        ///     -- or --
        ///     when <paramref name="instance"/> is not <see langword="null"/> but the field is a <see langword="static"/> field. 
        ///     -- or --
        ///     when <paramref name="value"/> is <see langword="null"/> but the field is not a nullable type (<see langword="class"/>, <see langword="interface"/> or <see cref="Nullable{T}"/>).
        /// </exception>
        public void SetValue(object? instance, object? value)
        {
            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static field.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static field.");

            if (value is null && !IsNullable)
                throw new InvalidOperationException(
                    $"The value cannot be null because {Member.DeclaringType}.{Member.Name} is of type {FieldType.Name}, which is not a nullable type (class, interface or Nullable<{FieldType.Name}>)");

            _set(instance, value);
        }
    }
}

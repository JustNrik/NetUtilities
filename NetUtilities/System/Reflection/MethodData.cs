using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class MethodData : MemberData<MethodInfo>
    {
        private readonly ConcurrentLazy<Action<object?, object?[]?>, (ParameterInfo[], MethodInfo)>? _action;
        private readonly ConcurrentLazy<Func<object?, object?[]?, object?>, (ParameterInfo[], MethodInfo)>? _func;

        /// <summary>
        ///     Gets the parameters of the method this data reflects.
        /// </summary>
        public ReadOnlyList<ParameterInfo> Parameters { get; init; }

        /// <summary>
        ///     Gets the generic arguments of the method this data reflects.
        /// </summary>
        public ReadOnlyList<Type> GenericArguments { get; init; }

        /// <summary>
        ///     Gets the attributes of the return type of the method this data reflects.
        /// </summary>
        public ReadOnlyList<Attribute> ReturnTypeAttributes { get; init; }

        /// <inheritdoc/>
        public MethodData(MethodInfo method) : base(method)
        {
            var parameters = method.GetParameters();
            Parameters = parameters.ToReadOnlyList();
            GenericArguments = method.GetGenericArguments().ToReadOnlyList();
            ReturnTypeAttributes = method.ReturnTypeCustomAttributes.GetCustomAttributes(false).Cast<Attribute>().ToReadOnlyList();

            if (method.ReturnType == typeof(void))
            {
                _action = new(static args =>
                {
                    var (parameters, method) = args;
                    var instance = Expression.Parameter(typeof(object));
                    var array = Expression.Parameter(typeof(object[]));
                    var indexes = parameters.Select((arg, index) => Expression.Convert(
                        Expression.ArrayIndex(
                            array,
                            Expression.Constant(index)),
                        arg.ParameterType)).ToArray();
                    var call = Expression.Call(
                        method.IsStatic 
                            ? null 
                            : Expression.Convert(instance, method.DeclaringType!), 
                        method,
                        indexes.Length == 0 
                            ? null 
                            : indexes);
                    return Expression.Lambda<Action<object?, object?[]?>>(call, array).Compile();
                }, (parameters, Member));
            }
            else
            {
                _func = new(static args =>
                {
                    var (parameters, method) = args;
                    var instance = Expression.Parameter(typeof(object));
                    var array = Expression.Parameter(typeof(object[]));
                    var indexes = parameters.Select((arg, index) => Expression.Convert(
                        Expression.ArrayIndex(
                            array,
                            Expression.Constant(index)),
                        arg.ParameterType)).ToArray();
                    var call = Expression.Call(
                        method.IsStatic 
                            ? null 
                            : Expression.Convert(instance, method.DeclaringType!),
                        method,
                        indexes.Length == 0 
                            ? null 
                            : indexes);
                    var convert = Expression.Convert(call, typeof(object));

                    return Expression.Lambda<Func<object?, object?[]?, object?>>(convert, array).Compile();
                }, (parameters, Member));
            }
        }

        /// <summary>
        ///     Invokes the method this data reflects.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the method is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must <b>not</b> be <see langword="null"/> if the method is <b>not</b> <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <param name="parameters">
        ///     The parameters the reflected method requires.
        /// </param>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the instance is <see langword="null"/> and the method is an instance method. 
        ///     -- or -- 
        ///     when the instance is not <see langword="null"/> and the method is <see langword="static"/>.
        ///     -- or -- 
        ///     when the parameters length doesn't match the method parameters count.
        /// </exception>
        /// <returns>
        ///     The value the method this data reflects returns. <see langword="null"/> if the return type of the reflected method is <see langword="void"/>.
        /// </returns>
        public virtual object? Invoke(object? instance)
        {
            if (instance is null && !Member.IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static method.");

            if (instance is not null && Member.IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static method.");

            return DirectInvoke(instance, null);
        }

        /// <summary>
        ///     Invokes the method this data reflects with the provided parameters.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the method is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must not be <see langword="null"/> if the method is not <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <param name="parameters">
        ///     The parameters the reflected method requires.
        /// </param>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the instance is <see langword="null"/> and the method is an instance method. 
        ///     -- or -- 
        ///     when the instance is not <see langword="null"/> and the method is <see langword="static"/>.
        ///     -- or -- 
        ///     when the parameters length doesn't match the method parameters count.
        /// </exception>
        /// <returns>
        ///     The value the method this data reflects returns. <see langword="null"/> if the return type of the reflected method is <see langword="void"/>.
        /// </returns>
        public virtual object? Invoke(object? instance, params object?[]? parameters)
        {
            if (parameters is null or { Length: 0 })
                return Invoke(instance);

            if (instance is null && !Member.IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static method.");

            if (instance is not null && Member.IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static method.");

            if (Parameters.Count != parameters.Length)
                throw new InvalidOperationException(
                    "The parameters length doesn't match the reflected method parameters count.");

            return DirectInvoke(instance, parameters);
        }

        protected virtual object? DirectInvoke(object? instance, object?[]? parameters)
        {
            if (Member.ReturnType == typeof(void))
            {
                _action!.Value(instance, parameters);
                return null;
            }

            return _func!.Value(instance, parameters);
        }
    }
}

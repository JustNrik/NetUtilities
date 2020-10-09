using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class MethodData : MemberData<MethodInfo>
    {
        private readonly Lazy<Action<object?, object?[]>> _action;
        private readonly Lazy<Func<object?, object[]?, object?>> _func;

        /// <summary>
        ///     Gets the parameters of the method this data reflects.
        /// </summary>
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        /// <summary>
        ///     Gets the generic arguments of the method this data reflects.
        /// </summary>
        public ReadOnlyList<Type> GenericArguments { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="MethodData"/> class 
        ///     with the provided <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">
        ///     The method.
        /// </param>
        public MethodData(MethodInfo method) : base(method)
        {
            Parameters = method.GetParameters().ToReadOnlyList();
            GenericArguments = method.GetGenericArguments().ToReadOnlyList();

            _func = new(() =>
            {
                var instance = Expression.Parameter(typeof(object));
                var array = Expression.Parameter(typeof(object[]));
                var parameters = Parameters.Select((arg, index) => Expression.Convert(
                    Expression.ArrayIndex(
                        array,
                        Expression.Constant(index)),
                    arg.ParameterType)).ToArray();
                var call = Expression.Call(instance, Member, parameters);
                var convert = Expression.Convert(call, typeof(object));

                return Expression.Lambda<Func<object?, object?[], object?>>(convert, array).Compile();
            }, true);
            _action = new(() => 
            {
                var instance = Expression.Parameter(typeof(object));
                var array = Expression.Parameter(typeof(object[]));
                var parameters = Parameters.Select((arg, index) => Expression.Convert(
                    Expression.ArrayIndex(
                        array,
                        Expression.Constant(index)),
                    arg.ParameterType)).ToArray();
                var call = Expression.Call(instance, Member, parameters);

                return Expression.Lambda<Action<object?, object?[]>>(call, array).Compile();
            },true);
        }

        /// <summary>
        ///     Invokes the method this data reflects.
        /// </summary>
        /// <param name="instance">
        ///     The instance object the method will be invoked on. 
        ///     Must be <see langword="null"/> if the method is <see langword="static"/>.
        /// </param>
        /// <param name="parameters">
        ///     The parameters the reflected method requires.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the instance is <see langword="null"/> and the method is an instance method. 
        ///     -- or -- 
        ///     when the instance is not <see langword="null"/> and the method is <see langword="static"/>.
        ///     -- or -- 
        ///     when the parameters length doesn't match the method parameters count.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <returns>
        ///     The value the method this data reflects returns. <see langword="null"/> if the return type of the reflected method is <see langword="void"/>.
        /// </returns>
        public object? Invoke(object? instance, params object?[]? parameters)
        {
            if (instance is null ^ Member.IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not an static method.");

            if (parameters is not null && Parameters.Count != parameters.Length)
                throw new InvalidOperationException(
                    "The parameters length doesn't match the reflected method parameters count.");

            if (_func.IsValueCreated)
                return _func.Value(instance, parameters);

            _action.Value(instance, parameters);
            return null;
        }
    }
}

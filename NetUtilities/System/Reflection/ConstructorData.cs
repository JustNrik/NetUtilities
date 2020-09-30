using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class ConstructorData : MemberData<ConstructorInfo>
    {
        private readonly Type _target;
        private Func<object[], object>? _constructor;

        /// <summary>
        ///     Gets the parameters of this constructor.
        /// </summary>
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="ConstructorData"/> class 
        ///     with the provided <see cref="ConstructorInfo"/> and target.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="target">The target.</param>
        public ConstructorData(ConstructorInfo constructor, Type target) : base(constructor)
        {
            _target = target;
            Parameters = constructor.GetParameters().ToReadOnlyList();
        }

        /// <summary>
        /// Creates an instance of this constructor's type.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throw if this constructors takes parameters or the type this constructor belongs to is <see langword="abstract"/> or <see langword="static"/>.</exception>
        /// <returns>An instance of the type this constructor belongs to.</returns>
        public object CreateInstance()
        {
            Ensure.CanOperate(!_target.IsAbstract, "You cannot create an instance of an abstract type.");
            Ensure.CanOperate(
                Parameters.Count == 0,
                $"This constructor {_target.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            return Factory.CreateInstance(_target);
        }

        /// <summary>
        /// Creates an instance of this constructor's type with the given arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when args is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the type this constructor belongs to is <see langword="abstract"/> or <see langword="static"/>.</exception>
        /// <exception cref="ParameterCountMismatchException">Thrown when the supplied arguments count is different from the parameters count of this constructor.</exception>
        /// <param name="args"></param>
        /// <returns></returns>
        [return: NotNull]
        public object CreateInstance(params object[] args)
        {
            Ensure.NotNull(args);
            Ensure.CanOperate(!_target.IsAbstract, "You cannot create an instance of an abstract type.");
            Ensure.CanOperate(
                Parameters.Count == args.Length,
                $"This constructor {_target.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            for (var index = 0; index < Parameters.Count; index++)
            {
                Ensure.CanOperate(
                    Parameters[index].ParameterType.IsAssignableFrom(args[index].GetType()),
                    $"The type {args[index].GetType().Name} cannot be assigned to {Parameters[index].ParameterType.Name}");
            }

            if (_constructor is not null)
                return _constructor(args);

            var array = Expression.Parameter(typeof(object[]));
            var parameters = args
                .Zip(Enumerable.Range(0, args.Length),
                    (arg, index) => Expression.Convert(
                        Expression.ArrayIndex(
                            array,
                            Expression.Constant(index)),
                        arg.GetType()))
                .ToArray();
            var @new = Expression.New(Member, parameters);
            var convert = Expression.Convert(@new, typeof(object));

            _constructor = Expression.Lambda<Func<object[], object>>(convert, array).Compile();
            return _constructor(args);
        }
    }
}

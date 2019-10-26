using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace System.Reflection
{
    public class ConstructorData : MemberData<ConstructorInfo>
    {
        private readonly Type _target;
        private Func<object[], object>? _constructor;
        public ReadOnlyList<ParameterInfo> Parameters { get; }

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
        [return: NotNull]
        public object CreateInstance()
        {
            if (_target.IsAbstract)
                Throw.InvalidOperation("You cannot create an instance of an abstract type.");

            if (Parameters.Count > 0)
                Throw.InvalidOperation($"This constructor {_target.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            return Factory.CreateInstance(_target);
        }

        /// <summary>
        /// Creates an instance of this constructor's type with the given arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when args is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the type this constructor belongs to is <see langword="abstract"/> or <see langword="static"/>.</exception>
        /// <exception cref="ParameterCountMismatchException">Thrown when the supplied arguments count is different from the parameters count of this constructor.</exception>
        /// <param name="args"></param>
        /// <returns></returns>
        [return: NotNull]
        public object CreateInstance([NotNull]params object[] args)
        {
            if (args is null)
                Throw.NullArgument(nameof(args));

            if (_target.IsAbstract)
                Throw.InvalidOperation("You cannot create an instance of an abstract type.");

            if (args.Length != Parameters.Count)
                Throw.ParameterCountMismatch($"This constructor {_target.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            for (var index = 0; index < args.Length; index++)
            {
                if (!args[index].GetType().CanBeConvertedTo(Parameters[index].ParameterType))
                    Throw.InvalidOperation($"The type {args[index].GetType().Name} cannot be implicitly converted to {Parameters[index].ParameterType.Name}");
            }

            if (_constructor is object)
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

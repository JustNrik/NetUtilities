using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class ConstructorData : MemberData<ConstructorInfo>
    {
        private readonly ConcurrentLazy<Func<object?[], object>, (ParameterInfo[], ConstructorInfo)>? _constructor;

        /// <summary>
        ///     Gets the parameters of the constructor this data reflects.
        /// </summary>
        public ReadOnlyList<ParameterInfo> Parameters { get; init; }

        /// <summary>
        ///     Gets the type that declared this constructor.
        /// </summary>
        /// <remarks>
        ///     This property will return <see langword="null"/> when:
        ///     <list type="number">
        ///         <item>
        ///             The constructor was retrieved from <see cref="Module"/>.
        ///         </item>
        ///         <item>
        ///             The constructor was declared in a <see langword="Module"/> (Visual Basic .NET).
        ///         </item>
        ///     </list>
        /// </remarks>
        public Type? DeclaringType { get; init; }

        /// <summary>
        ///     Indicates if this constructor data reflects the default constructor.
        /// </summary>
        [MemberNotNullWhen(false, nameof(_constructor))]
        public bool IsDefault
            => Parameters.Count == 0;

        /// <summary>
        ///     Initializes a new instance of <see cref="ConstructorData"/> class 
        ///     with the provided <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="constructor">
        ///     The constructor.
        /// </param>
        /// <param name="target">
        ///     The target.
        /// </param>
        public ConstructorData(ConstructorInfo constructor) : base(constructor)
        {
            Ensure.NotNull(constructor);

            var parameters = constructor.GetParameters();

            DeclaringType = constructor.DeclaringType;
            Parameters = parameters.ToReadOnlyList();

            // Nothing to do if the type is a module, abstract or the constructor is a default constructor.
            if (DeclaringType is null or { IsAbstract: true } || Parameters.Count == 0)
                return;

            _constructor = new(static args =>
            {
                var (parameterInfos, member) = args;
                var array = Expression.Parameter(typeof(object[]));
                var indexes = parameterInfos.Select((parameter, index) => Expression.Convert(
                    Expression.ArrayIndex(
                        array,
                        Expression.Constant(index)),
                    parameter.ParameterType));
                var @new = Expression.New(member, indexes);
                var convert = Expression.Convert(@new, typeof(object));

                return Expression.Lambda<Func<object?[], object>>(convert, array).Compile();
            }, (parameters, Member));
        }

        /// <summary>
        ///     Creates an instance of the type this constructor belongs to.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Throw if this constructors requires parameters 
        ///     -- OR -- 
        ///     the type this constructor belongs to is either <see langword="abstract"/> or <see langword="static"/>.
        /// </exception>
        /// <returns>
        ///     An instance of the type this constructor belongs to.
        /// </returns>
        public object CreateInstance()
        {
            if (DeclaringType is null)
                throw new InvalidOperationException("Cannot create an instance of an unknown type or a Module");

            if (DeclaringType.IsAbstract)
                throw new InvalidOperationException(
                    $"You cannot create an instance of an {(DeclaringType.IsSealed ? "static" : "abstract")} class.");

            if (!IsDefault)
                throw new InvalidOperationException(
                    $"This constructor {DeclaringType.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            return Factory.CreateInstance(DeclaringType);
        }

        /// <summary>
        ///     Creates an instance of the type this constructor belongs to with the provided arguments.
        /// </summary>
        /// <param name="args">
        ///     The arguments.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the supplied arguments count is different from the parameters count of this constructor.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the type this constructor belongs to is either <see langword="abstract"/> or <see langword="static"/>.
        /// </exception>
        /// <returns>
        ///     An instance of the type this constructor belongs to.
        /// </returns>
        public object CreateInstance(params object?[]? args)
        {
            if (args is null or { Length: 0 })
                return CreateInstance();

            if (DeclaringType is null)
                throw new InvalidOperationException("Cannot create an instance of an unknown type or a Module.");

            if (DeclaringType.IsAbstract)
                throw new InvalidOperationException(
                    $"You cannot create an instance of an {(DeclaringType.IsSealed ? "static" : "abstract")} type.");

            if (Parameters.Count != args.Length)
                throw new InvalidOperationException(
                    $"This constructor {DeclaringType.Name}({string.Join(", ", Parameters.Select(x => x.ParameterType.Name))}) requires these parameters to be used.");

            return _constructor!.Value(args);
        }
    }
}

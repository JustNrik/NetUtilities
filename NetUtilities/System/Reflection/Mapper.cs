using System.Runtime.Versioning;
using NetUtilities;

namespace System.Reflection
{
    /// <summary>
    ///     Handy class to map reflection metadata and provide high performance runtime manipulation.
    /// </summary>
    // Supported only on windows until we figure out why it doesn't work on MacOS. Need to test in linux as well?
    [SupportedOSPlatform("windows")]
    public sealed class Mapper
    {
        private readonly ConcurrentLazy<ReadOnlyList<Attribute>, Type> _attributes;
        private readonly ConcurrentLazy<ReadOnlyList<CustomAttributeData>, Type> _customAttributeDatas;
        private readonly ConcurrentLazy<ReadOnlyList<ConstructorData>, Type> _constructors;
        private readonly ConcurrentLazy<ReadOnlyList<EventData>, Type> _events;
        private readonly ConcurrentLazy<ReadOnlyList<FieldData>, Type> _fields;
        private readonly ConcurrentLazy<ReadOnlyList<MethodData>, Type> _methods;
        private readonly ConcurrentLazy<ReadOnlyList<PropertyData>, Type> _properties;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="Attribute"/>&gt; that contains data related to the type's custom attributes.
        /// </summary>
        public ReadOnlyList<Attribute> Attributes
            => _attributes.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="CustomAttributeData"/>&gt; that contains data related to the type's custom attribute data for assemblies, modules, 
        ///     types, members and parameters that are loaded into the reflection-only context.
        /// </summary>
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _customAttributeDatas.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="ConstructorData"/>&gt; that contains data related to the type's constructors
        /// </summary>
        public ReadOnlyList<ConstructorData> Constructors
            => _constructors.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="EventData"/>&gt; that contains data related to the type's events
        /// </summary>
        public ReadOnlyList<EventData> Events
            => _events.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="FieldData"/>&gt; that contains data related to the type's fields
        /// </summary>
        public ReadOnlyList<FieldData> Fields
            => _fields.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="MethodData"/>&gt; that contains data related to the type's methods
        /// </summary>
        public ReadOnlyList<MethodData> Methods
            => _methods.Value;

        /// <summary>
        ///     Gets a <see cref="ReadOnlyList"/>&lt;<see cref="PropertyData"/>&gt; that contains data related to the type's properties
        /// </summary>
        public ReadOnlyList<PropertyData> Properties
            => _properties.Value;

        /// <summary>
        ///     Initializes a new instance of <see cref="Mapper"/> class with the provided instance object.
        /// </summary>
        /// <param name="object">
        ///     The object whose's type is to be mapped.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="object"/> is <see langword="null"/>.
        /// </exception>
        public Mapper(object @object) : this(Ensure.NotNull(@object).GetType())
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Mapper"/> class with the provided type.
        /// </summary>
        /// <param name="type">
        ///     The type to be mapped.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="type"/> is <see langword="null"/>
        /// </exception>
        public Mapper(Type type)
        {
            _attributes = new(static type => type.GetCustomAttributes().ToReadOnlyList(), type);
            _customAttributeDatas = new(static type => type.CustomAttributes.ToReadOnlyList(), type);
            _constructors = new(static type => type.GetRuntimeConstructors().Select(x => new ConstructorData(x)).ToReadOnlyList(), type);
            _events = new(static type => type.GetRuntimeEvents().Select(x => new EventData(x)).ToReadOnlyList(), type);
            _fields = new(static type => type.GetRuntimeFields().Select(x => new FieldData(x)).ToReadOnlyList(), type);
            _methods = new(static type => type.GetRuntimeMethods().Select(x => new MethodData(x)).ToReadOnlyList(), type);
            _properties = new(static type => type.GetRuntimeProperties().Select(x => new PropertyData(x)).ToReadOnlyList(), type);
        }
    }

    /// <summary>
    ///     Handy class to map reflection metadata and provide high performance runtime manipulation.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to be mapped.
    /// </typeparam>
    // Supported only on windows until we figure out why it doesn't work on MacOS. Need to test in linux as well?
    [SupportedOSPlatform("windows")]
    public static class Mapper<T>
    {
        private static readonly Mapper _mapper = new(typeof(T));

        /// <inheritdoc cref="Mapper.Attributes"/>
        public static ReadOnlyList<Attribute> Attributes
            => _mapper.Attributes;

        /// <inheritdoc cref="Mapper.CustomAttributeDatas"/>
        public static ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _mapper.CustomAttributeDatas;

        /// <inheritdoc cref="Mapper.Constructors"/>
        public static ReadOnlyList<ConstructorData> Constructors
            => _mapper.Constructors;

        /// <inheritdoc cref="Mapper.Events"/>
        public static ReadOnlyList<EventData> Events
            => _mapper.Events;

        /// <inheritdoc cref="Mapper.Fields"/>
        public static ReadOnlyList<FieldData> Fields
            => _mapper.Fields;

        /// <inheritdoc cref="Mapper.Methods"/>
        public static ReadOnlyList<MethodData> Methods
            => _mapper.Methods;

        /// <inheritdoc cref="Mapper.Properties"/>
        public static ReadOnlyList<PropertyData> Properties
            => _mapper.Properties;
    }
}

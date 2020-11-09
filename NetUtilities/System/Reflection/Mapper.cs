using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetUtilities;

namespace System.Reflection
{
    /// <summary>
    ///     Handy class to map reflection metadata and provide high performance runtime manipulation.
    /// </summary>
    public sealed class Mapper
    {
        private const BindingFlags All = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

        private readonly Type _target;
        private readonly Lazy<ReadOnlyList<Attribute>> _customAttributes;
        private readonly Lazy<ReadOnlyList<CustomAttributeData>> _customAttributeDatas;
        private readonly Lazy<ReadOnlyList<ConstructorData>> _constructors;
        private readonly Lazy<ReadOnlyList<EventData>> _events;
        private readonly Lazy<ReadOnlyList<FieldData>> _fields;
        private readonly Lazy<ReadOnlyList<MethodData>> _methods;
        private readonly Lazy<ReadOnlyList<PropertyData>> _properties;

        /// <summary>
        ///     Contains data related to the type's custom attributes.
        /// </summary>
        public ReadOnlyList<Attribute> CustomAttributes
            => _customAttributes.Value;

        /// <summary>
        ///     Contains data related to the type's custom attribute data for assemblies, modules, 
        ///     types, members and parameters that are loaded into the reflection-only context.
        /// </summary>
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _customAttributeDatas.Value;

        /// <summary>
        ///     Contains data related to the type's constructors
        /// </summary>
        public ReadOnlyList<ConstructorData> Constructors
            => _constructors.Value;

        /// <summary>
        ///     Contains data related to the type's events
        /// </summary>
        public ReadOnlyList<EventData> Events
            => _events.Value;

        /// <summary>
        ///     Contains data related to the type's fields
        /// </summary>
        public ReadOnlyList<FieldData> Fields
            => _fields.Value;

        /// <summary>
        ///     Contains data related to the type's methods
        /// </summary>
        public ReadOnlyList<MethodData> Methods
            => _methods.Value;

        /// <summary>
        ///     Contains data related to the type's properties
        /// </summary>
        public ReadOnlyList<PropertyData> Properties
            => _properties.Value;

        /// <summary>
        ///     Initializes a new instance of <see cref="Mapper"/> class with the provided instance object.
        /// </summary>
        /// <param name="object">
        ///     The object.
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
        ///     The type.
        /// </param>
        public Mapper(Type type)
        {
            Ensure.NotNull(type);

            _target = type;
            _customAttributes = new(() => _target.GetCustomAttributes().ToReadOnlyList(), true);
            _customAttributeDatas = new(() => _target.CustomAttributes.ToReadOnlyList(), true);
            _constructors = new(() => _target.GetRuntimeConstructors().Select(x => new ConstructorData(x, _target)).ToReadOnlyList(), true);
            _events = new(() => _target.GetRuntimeEvents().Select(x => new EventData(x)).ToReadOnlyList(), true);
            _fields = new(() => _target.GetRuntimeFields().Select(x => new FieldData(x)).ToReadOnlyList(), true);
            _methods = new(() => _target.GetRuntimeMethods().Select(x => new MethodData(x)).ToReadOnlyList(), true);
            _properties = new(() => _target.GetRuntimeProperties().Select(x => new PropertyData(x)).ToReadOnlyList(), true);
        }
    }

    /// <summary>
    ///     Handy class to map reflection metadata and provide high performance runtime manipulation.
    /// </summary>
    /// <typeparam name="T">
    ///     The type.
    /// </typeparam>
    public static class Mapper<T> 
    {
        private static readonly Mapper _mapper = new Mapper(typeof(T));

        /// <summary>
        ///     Contains data related to the <typeparamref name="T"/>'s custom attributes.
        /// </summary>
        public static ReadOnlyList<Attribute> CustomAttributes
            => _mapper.CustomAttributes;

        /// <summary>
        ///     Contains data related to the <typeparamref name="T"/>'s custom attribute data for assemblies, modules, 
        ///     types, members and parameters that are loaded into the reflection-only context.
        /// </summary>
        public static ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _mapper.CustomAttributeDatas;

        /// <summary>
        ///     Gets the <see cref="ConstructorData"/> for all constructors of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<ConstructorData> Constructors 
            => _mapper.Constructors;

        /// <summary>
        ///     Gets the <see cref="EventData"/> for all events of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<EventData> Events 
            => _mapper.Events;

        /// <summary>
        ///     Gets the <see cref="FieldData"/> for all fields of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<FieldData> Fields 
            => _mapper.Fields;

        /// <summary>
        ///     Gets the <see cref="MethodData"/> for all methods of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<MethodData> Methods 
            => _mapper.Methods;

        /// <summary>
        ///     Gets the <see cref="PropertyData"/> for all properties of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<PropertyData> Properties 
            => _mapper.Properties;
    }
}

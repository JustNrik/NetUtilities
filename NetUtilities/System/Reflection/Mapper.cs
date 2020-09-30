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
        private Lazy<ReadOnlyList<ConstructorData>> _constructors;
        private Lazy<ReadOnlyList<EventData>> _events;
        private Lazy<ReadOnlyList<FieldData>> _fields;
        private Lazy<ReadOnlyList<MethodData>> _methods;
        private Lazy<ReadOnlyList<MethodData>> _methodsExcludingObjectBaseMembers;
        private Lazy<ReadOnlyList<MethodData>> _methodsDeclaringTypeOnly;
        private Lazy<ReadOnlyList<PropertyData>> _properties;

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
        ///     Contains data related to the type's methods - excluding members inhereted from the Object base class
        /// </summary>
        public ReadOnlyList<MethodData> MethodsExcludingObjectBaseMembers
            => _methodsExcludingObjectBaseMembers.Value;

        /// <summary>
        ///     Contains data related to the type's methods - excluding all inhereted members
        /// </summary>
        public ReadOnlyList<MethodData> MethodsDeclaringTypeOnly
            => _methodsDeclaringTypeOnly.Value;

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
            _constructors = new(() => _target.GetConstructors(All).Select(x => new ConstructorData(x, _target)).ToReadOnlyList(), true);
            _events = new(() => _target.GetRuntimeEvents().Select(x => new EventData(x)).ToReadOnlyList(), true);
            _fields = new(() => _target.GetRuntimeFields().Select(x => new FieldData(x)).ToReadOnlyList(), true);
            _methods = new(() => _target.GetRuntimeMethods().Select(x => new MethodData(x)).ToReadOnlyList(), true);
            _methodsExcludingObjectBaseMembers = new(() => _methods.Value
                .FindAll(x => x.Member.DeclaringType != typeof(object)), true);
            _methodsDeclaringTypeOnly = new(() => _methods.Value
                .FindAll(x => x.Member.DeclaringType == _target), true);
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
        ///     Gets the data about all constructors of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<ConstructorData> Constructors 
            => _mapper.Constructors;

        /// <summary>
        ///     Gets the data about all events of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<EventData> Events 
            => _mapper.Events;

        /// <summary>
        ///     Gets the data about all fields of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<FieldData> Fields 
            => _mapper.Fields;

        /// <summary>
        ///     Gets the data about all methods of <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<MethodData> Methods 
            => _mapper.Methods;

        /// <summary>
        ///     Gets the data about all methods of <typeparamref name="T"/> excluding those inherited from <see cref="object"/>.
        /// </summary>
        public static ReadOnlyList<MethodData> MethodsExcludingObjectBaseMembers
            => _mapper.MethodsExcludingObjectBaseMembers;

        /// <summary>
        ///     Gets the data about all methods declared by <typeparamref name="T"/>.
        /// </summary>
        public static ReadOnlyList<MethodData> MethodsDeclaringTypeOnly
            => _mapper.MethodsDeclaringTypeOnly;

        /// <summary>
        /// Contains data related to <typeparamref name="T"/>'s properties.
        /// </summary>
        public static ReadOnlyList<PropertyData> Properties 
            => _mapper.Properties;
    }
}

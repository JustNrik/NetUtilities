using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// Handy class to map reflection metadata and provide high performance runtime manipulation.
    /// </summary>
    public sealed class Mapper 
    {
        private const BindingFlags All = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

        private readonly Type _source;
        private ReadOnlyList<ConstructorData>? _constructors;
        private ReadOnlyList<EventData>? _events;
        private ReadOnlyList<FieldData>? _fields;
        private ReadOnlyList<MethodData>? _methods;
        private ReadOnlyList<PropertyData>? _properties;

        /// <summary>
        /// Contains data related to the type's constructors
        /// </summary>
        public ReadOnlyList<ConstructorData> Constructors
            => _constructors ?? (_constructors = _source.GetConstructors(All).Select(x => new ConstructorData(x, _source)).ToReadOnlyList());

        /// <summary>
        /// Contains data related to the type's events
        /// </summary>
        public ReadOnlyList<EventData> Events 
            => _events ?? (_events = _source.GetEvents(All).Select(x => new EventData(x)).ToReadOnlyList());

        /// <summary>
        /// Contains data related to the type's fields
        /// </summary>
        public ReadOnlyList<FieldData> Fields
            => _fields ?? (_fields = _source.GetFields(All).Select(x => new FieldData(x)).ToReadOnlyList());

        /// <summary>
        /// Contains data related to the type's methods
        /// </summary>
        public ReadOnlyList<MethodData> Methods
            => _methods ?? (_methods = _source.GetMethods(All).Select(x => new MethodData(x)).ToReadOnlyList());

        /// <summary>
        /// Contains data related to the type's properties
        /// </summary>
        public ReadOnlyList<PropertyData> Properties
            => _properties ?? (_properties = _source.GetProperties(All).Select(x => new PropertyData(x)).ToReadOnlyList());

        /// <summary>
        /// Creates the mapper for the given object.
        /// </summary>
        /// <param name="object">The object.</param>
        public Mapper([NotNull]object @object) : this(Ensure.NotNull(@object, nameof(@object)).GetType())
        {
        }

        /// <summary>
        /// Creates the mapper for the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        public Mapper([NotNull]Type type)
        {
            if (type is null)
                Throw.NullArgument(nameof(type));

            _source = type;
        }
    }

    /// <summary>
    /// Handy class to map reflection metadata and provide high performance runtime manupulation.
    /// </summary>
    /// <typeparam name="TMember">The type of the <see cref="MemberInfo"/></typeparam>
    public static class Mapper<TMember> where TMember : MemberInfo
    {
        private static readonly Mapper _mapper = new Mapper(typeof(TMember));

        /// <summary>
        /// Contains data related to <typeparamref name="TMember"/>'s constructors.
        /// </summary>
        public static ReadOnlyList<ConstructorData> Constructors => _mapper.Constructors;

        /// <summary>
        /// Contains data related to <typeparamref name="TMember"/>'s events.
        /// </summary>
        public static ReadOnlyList<EventData> Events => _mapper.Events;

        /// <summary>
        /// Contains data related to <typeparamref name="TMember"/>'s fields.
        /// </summary>
        public static ReadOnlyList<FieldData> Fields => _mapper.Fields;

        /// <summary>
        /// Contains data related to <typeparamref name="TMember"/>'s methods.
        /// </summary>
        public static ReadOnlyList<MethodData> Methods => _mapper.Methods;

        /// <summary>
        /// Contains data related to <typeparamref name="TMember"/>'s properties.
        /// </summary>
        public static ReadOnlyList<PropertyData> Properties => _mapper.Properties;
    }
}

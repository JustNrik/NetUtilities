using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// Handy class to map reflection metadata. This class has a heavy instantiaton logic, make sure to cache this class.
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
            => _constructors ?? (_constructors = _source.GetConstructors(All).Select(x => new ConstructorData(x)).ToReadOnlyList());

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
}

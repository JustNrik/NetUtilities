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

        /// <summary>
        /// Contains data related to the type's constructors
        /// </summary>
        public ReadOnlyList<MemberData<ConstructorInfo>> Constructors { get; }

        /// <summary>
        /// Contains data related to the type's events
        /// </summary>
        public ReadOnlyList<MemberData<EventInfo>> Events { get; }

        /// <summary>
        /// Contains data related to the type's fields
        /// </summary>
        public ReadOnlyList<MemberData<FieldInfo>> Fields { get; }

        /// <summary>
        /// Contains data related to the type's methods
        /// </summary>
        public ReadOnlyList<MemberData<MethodInfo>> Methods { get; }

        /// <summary>
        /// Contains data related to the type's properties
        /// </summary>
        public ReadOnlyList<MemberData<PropertyInfo>> Properties { get; }

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

            Fields = type.GetFields(All).Select(field => new MemberData<FieldInfo>(field)).ToReadOnlyList();
            Properties = type.GetProperties(All).Select(property => new MemberData<PropertyInfo>(property)).ToReadOnlyList();
            Constructors = type.GetConstructors(All).Select(constructor => new MemberData<ConstructorInfo>(constructor)).ToReadOnlyList();
            Methods = type.GetMethods(All).Select(method => new MemberData<MethodInfo>(method)).ToReadOnlyList();
            Events = type.GetEvents(All).Select(@event => new MemberData<EventInfo>(@event)).ToReadOnlyList();
        }
    }
}

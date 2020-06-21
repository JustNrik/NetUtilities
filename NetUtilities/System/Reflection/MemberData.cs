﻿using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    ///     Contains rich data and privides helper methods for a <typeparamref name="TMember"/>.
    /// </summary>
    /// <typeparam name="TMember">
    ///     The <see cref="MemberInfo"/> type.
    /// </typeparam>
    public abstract class MemberData<TMember> where TMember : MemberInfo
    {
        private Lazy<ReadOnlyList<Attribute>> _attributes;
        private Lazy<ReadOnlyList<CustomAttributeData>> _customAttributeDatas;

        /// <summary>
        /// Gets the <see cref="MemberInfo"/> for this class.
        /// </summary>
        public TMember Member { get; }

        /// <summary>
        /// Gets the custom attributes of this member.
        /// </summary>
        public ReadOnlyList<Attribute> Attributes
            => _attributes.Value;

        /// <summary>
        /// Gets the custom attribute datas of this member.
        /// </summary>
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _customAttributeDatas.Value;

        protected MemberData(TMember member)
        {
            Member = member;
            _attributes = new(() => Member.GetCustomAttributes().ToReadOnlyList(), true);
            _customAttributeDatas = new(() => Member.CustomAttributes.ToReadOnlyList(), true);

        }
    }
}

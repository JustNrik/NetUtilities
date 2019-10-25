using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public abstract class MemberData<TMember> where TMember : MemberInfo
    {
        private ReadOnlyList<Attribute>? _attributes;
        private ReadOnlyList<CustomAttributeData>? _customAttributeDatas;

        public TMember Member { get; }

        public ReadOnlyList<Attribute> Attributes
            => _attributes ?? (_attributes = Member.GetCustomAttributes().ToReadOnlyList());
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _customAttributeDatas ?? (_customAttributeDatas = Member.CustomAttributes.ToReadOnlyList());

        protected MemberData(TMember member)
        {
            Member = member;
        }
    }
}

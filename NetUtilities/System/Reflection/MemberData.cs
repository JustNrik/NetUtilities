using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public sealed class MemberData<TMember> where TMember : MemberInfo
    {
        public TMember Member { get; }
        public ReadOnlyList<Attribute> Attributes { get; }
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas { get; }
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public MemberData(TMember member)
        {
            Member = member;
            Attributes = member.GetCustomAttributes().ToReadOnlyList();
            CustomAttributeDatas = member.CustomAttributes.ToReadOnlyList();
            Parameters = member switch
            {
                PropertyInfo property => property.GetIndexParameters().ToReadOnlyList(),
                MethodInfo method => method.GetParameters().ToReadOnlyList(),
                _ => new ReadOnlyList<ParameterInfo>()
            };
        }
    }
}

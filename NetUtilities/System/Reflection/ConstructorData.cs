using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public class ConstructorData : MemberData
    {
        public new ConstructorInfo Member => (ConstructorInfo)base.Member;
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public ConstructorData(ConstructorInfo constructor) : base(constructor)
        {
            Parameters = constructor.GetParameters().ToReadOnlyList();
        }
    }
}

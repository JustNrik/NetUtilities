using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public class ConstructorData : MemberData<ConstructorInfo>
    {
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public ConstructorData(ConstructorInfo constructor) : base(constructor)
        {
            Parameters = constructor.GetParameters().ToReadOnlyList();
        }
    }
}

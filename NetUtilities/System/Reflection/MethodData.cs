using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public class MethodData : MemberData<MethodInfo>
    {
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public MethodData(MethodInfo method) : base(method)
        {
            Parameters = method.GetParameters().ToReadOnlyList();
        }
    }
}

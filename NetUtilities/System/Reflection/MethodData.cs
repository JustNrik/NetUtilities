using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public class MethodData : MemberData
    {
        public new MethodInfo Member => (MethodInfo)base.Member;
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public MethodData(MethodInfo method) : base(method)
        {
            Parameters = method.GetParameters().ToReadOnlyList();
        }
    }
}

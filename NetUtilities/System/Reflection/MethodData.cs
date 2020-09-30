using System.Collections.Generic;
using System.Linq;
using NetUtilities;

namespace System.Reflection
{
    public class MethodData : MemberData<MethodInfo>
    {
        //private readonly Func<object[]?, object?> _func;
        public ReadOnlyList<ParameterInfo> Parameters { get; }

        public MethodData(MethodInfo method) : base(method)
        {
            Parameters = method.GetParameters().ToReadOnlyList();
        }
        /* TODO
        /// <summary>
        ///     Invokes the method this data reflects.
        /// </summary>
        public object? Invoke(params object[] parameters)
        {
        }*/
    }
}

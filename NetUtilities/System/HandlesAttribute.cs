using NetUtilities;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HandlesAttribute : Attribute
    {
        public Type EventSourceType { get; }
        public EventInfo EventInfo { get; }
        public string MethodName { get; }

        public HandlesAttribute(Type eventSourceType, string eventName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, [CallerMemberName]string methodName = "")
        {
            EventSourceType = Ensure.NotNull(eventSourceType, nameof(eventSourceType));
            MethodName = methodName;
            EventInfo = eventSourceType.GetEvent(eventName, flags);

            if (EventInfo is null)
                throw new EventNotFoundException(eventName, eventSourceType.Name, flags);
        }
    }
}

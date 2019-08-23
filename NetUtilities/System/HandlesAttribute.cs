namespace System
{
    using NetUtilities;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class HandlesAttribute : Attribute
    {
        public EventInfo EventInfo { get; }

        public HandlesAttribute(Type eventSourceType, string eventName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance, [CallerMemberName]string methodName = "")
        {
            Ensure.NotNull(eventSourceType, nameof(eventSourceType));
            EventInfo = eventSourceType.GetEvent(eventName, flags);

            if (EventInfo is null)
                throw new EventNotFoundException(eventName, eventSourceType.Name, flags);
        }
    }
}

namespace System
{
    using System.Reflection;

    public sealed class EventNotFoundException : Exception
    {
        public EventNotFoundException(string eventName, string objectName, BindingFlags flags) : base(
            $"The event {eventName} couldn't be found in the object {objectName} with the flags {flags}")
        {
        }
    }
}

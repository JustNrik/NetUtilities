namespace System.Reflection
{
    public class EventData : MemberData<EventInfo>
    {
        public Type EventHandlerType { get; }

        public EventData(EventInfo @event) : base(@event)
        {
            EventHandlerType = @event.EventHandlerType;
        }

        //public void AddHandler(object source, object target, Delegate method)
        //{

        //}
    }
}

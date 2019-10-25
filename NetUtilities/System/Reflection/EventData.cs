namespace System.Reflection
{
    public class EventData : MemberData
    {
        public new EventInfo Member => (EventInfo)base.Member;
        public Type EventHandlerType => Member.EventHandlerType;

        public EventData(EventInfo @event) : base(@event)
        {
        }
    }
}

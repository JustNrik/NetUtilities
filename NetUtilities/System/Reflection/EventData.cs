﻿namespace System.Reflection
{
    // TODO: Implement this.
    /// <inheritdoc/>
    public class EventData : MemberData<EventInfo>
    {
        //private readonly Lazy<Func<object?, object?[]?>?> _raise;
        //private readonly Lazy<Action<Delegate>> _add;
        //private readonly Lazy<Action<Delegate>> _remove;
        public Type EventHandlerType { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="EventData"/> class 
        ///     with the provided <see cref="EventInfo"/> and target.
        /// </summary>
        /// <param name="event">
        ///     The event.
        /// </param>
        public EventData(EventInfo @event) : base(@event)
        {
            EventHandlerType = @event.EventHandlerType;
            //@event.GetRaiseMethod();
            //@event.GetAddMethod();
            //@event.GetRemoveMethod();
        }

        private object RaiseEvent(object target, params object?[] arguments)
        {
            throw new System.NotImplementedException();
        }

        private void AddHandler(object target, Delegate handler)
        {
            throw new System.NotImplementedException();
        }

        private void RemoveHandler(object target, Delegate handler)
        {
            throw new System.NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetUtilities;

namespace System.Reflection
{
    /// <inheritdoc/>
    public class EventData : MemberData<EventInfo>
    {
        private readonly Lazy<Action<object?, Delegate>> _add;
        private readonly Lazy<Action<object?, Delegate>> _remove;

        /// <summary>
        ///     Gets the <see cref="Type"/> object of the underlying event-handler delegate associated with this event.
        /// </summary>
        public Type EventHandlerType { get; }

        /// <summary>
        ///     Indicates if the <see langword="event"/> this data reflects to is <see langword="static"/>.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        ///     Initializes a new instance of <see cref="EventData"/> class with the provided <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event">
        ///     The event.
        /// </param>
        public EventData(EventInfo @event) : base(@event)
        {
            EventHandlerType = @event.EventHandlerType;
            IsStatic = @event.DeclaringType!.GetRuntimeField(@event.Name)!.IsStatic;

            _add = new(() =>
            {
                var instance = Expression.Parameter(typeof(object));
                var handler = Expression.Parameter(typeof(Delegate));
                var cast = Expression.Convert(instance, @event.DeclaringType!);
                var delegateCast = Expression.Convert(handler, EventHandlerType!);
                var call = Expression.Call(cast, @event.AddMethod, delegateCast);
                var lambda = Expression.Lambda<Action<object?, Delegate>>(call, instance, handler);
                return lambda.Compile();
            }, true);
            _remove = new(() =>
            {
                var instance = Expression.Parameter(typeof(object));
                var handler = Expression.Parameter(typeof(Delegate));
                var cast = Expression.Convert(instance, @event.DeclaringType);
                var delegateCast = Expression.Convert(handler, EventHandlerType);
                var call = Expression.Call(cast, @event.RemoveMethod, delegateCast);
                var lambda = Expression.Lambda<Action<object?, Delegate>>(call, instance, handler);
                return lambda.Compile();
            }, true);
        }

        private object RaiseEvent(object? instance, params object?[] parameters)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     Adds a handler to the event this data reflects.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the event is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must <b>not</b> be <see langword="null"/> if the event is <b>not</b> <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <param name="handler">
        ///     The handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="instance"/> is <see langword="null"/> and the event is an instance event. 
        ///     -- or -- 
        ///     when the <paramref name="instance"/> is not <see langword="null"/> and the event is <see langword="static"/>.
        /// </exception>
        public void AddHandler(object? instance, Delegate handler)
        {
            Ensure.NotNull(handler);

            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static event.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static event.");

            _add.Value(instance, handler);
        }

        /// <summary>
        ///     Removes a handler to the event this data reflects.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="instance"/> must be <see langword="null"/> if the event is <see langword="static"/>.<br/>
        ///     The <paramref name="instance"/> must <b>not</b> be <see langword="null"/> if the event is <b>not</b> <see langword="static"/>.
        /// </remarks>
        /// <param name="instance">
        ///     The instance object.
        /// </param>
        /// <param name="handler">
        ///     The handler.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="handler"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     Thrown when one of the arguments couldn't be casted to the respective type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when <paramref name="instance"/> is <see langword="null"/> and the event is an instance event. 
        ///     -- or -- 
        ///     when the <paramref name="instance"/> is not <see langword="null"/> and the event is <see langword="static"/>.
        /// </exception>
        public void RemoveHandler(object? instance, Delegate handler)
        {
            Ensure.NotNull(handler);

            if (instance is null && !IsStatic)
                throw new InvalidOperationException(
                    $"The instance cannot be null because {Member.DeclaringType}.{Member.Name} is not a static event.");

            if (instance is not null && IsStatic)
                throw new InvalidOperationException(
                    $"The instance must be null because {Member.DeclaringType}.{Member.Name} is a static event.");

            _remove.Value(instance, handler);
        }
    }
}

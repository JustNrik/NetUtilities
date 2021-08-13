using System.Reflection;
using NetUtilities;

namespace System
{
    /// <summary>
    ///     This class is a handy wrapper for automatic event wrapping.
    ///     You shouldn't use this class to dynamically add/remove handlers frequently
    ///     as this class relys on <see cref="Reflection"/> which may negatively affect
    ///     the performance of your application.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The source of the events
    /// </typeparam>
    public class EventManager<TSource> where TSource : notnull
    {
        private readonly Dictionary<object, List<(EventInfo, Delegate)>> _handlers = new();

        /// <summary>
        ///     The source of the events
        /// </summary>
        public TSource Source { get; init; }

        /// <summary>
        ///     Creates an instance of <see cref="EventManager{TSource}"/> with the instance of the source provided.
        /// </summary>
        /// <param name="source">
        ///     The source of the events.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        public EventManager(TSource source)
            => Source = Ensure.NotNull(source);


        /// <summary>
        ///     Adds all the handlers to the methods that have <see cref="HandlesAttribute"/>.
        /// </summary>
        /// <param name="target">
        ///     The instance of object that will listen to the events.
        /// </param>
        /// <param name="flags">
        ///     The flags used for <see cref="Reflection"/> to search the methods that will listen to the events.
        /// </param>
        public virtual void AddHandlers(object target, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var targetType = target.GetType();
            var metadatas = targetType.GetMethods(flags)
                .Select(method => new
                {
                    Method = method,
                    Events = method.GetCustomAttributes<HandlesAttribute>().Select(attribute => attribute.EventInfo).ToArray()
                })
                .Where(metadata => metadata.Events.Length > 0);

            foreach (var metadata in metadatas)
                foreach (var eventInfo in metadata.Events)
                {
                    var handler = metadata.Method.CreateDelegate(eventInfo.EventHandlerType!, target);
                    eventInfo.AddEventHandler(Source, handler);

                    if (_handlers.TryGetValue(target, out var list))
                        list.Add((eventInfo, handler));
                    else
                        _handlers.Add(target, new List<(EventInfo, Delegate)>() { (eventInfo, handler) });
                }
        }

        /// <summary>
        ///     Removes all handlers to the methods with <see cref="HandlesAttribute"/>.
        /// </summary>
        /// <param name="target">
        ///     The instance of object that is currently listening to the events.
        /// </param>
        public virtual void RemoveHandlers(object target)
        {
            if (!_handlers.TryGetValue(target, out var list))
                return;

            foreach (var (eventInfo, handler) in list)
                eventInfo.RemoveEventHandler(target, handler);
        }
    }
}

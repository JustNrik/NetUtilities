namespace System
{
    /// <summary>
    ///     Represents the method that will handle an event of <typeparamref name="TEventArgs"/> data from <typeparamref name="TSender"/>.
    /// </summary>
    /// <typeparam name="TSender">
    ///     The sender type.
    /// </typeparam>
    /// <typeparam name="TEventArgs">
    ///     The event data type.
    /// </typeparam>
    /// <param name="sender">
    ///     The sender.
    /// </param>
    /// <param name="eventArgs">
    ///     The event data.
    /// </param>
    public delegate void EventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs eventArgs);
}

namespace FunnelWeb.Eventing
{
    /// <summary>
    /// Used to listen on events in the system
    /// </summary>
    /// <typeparam name="T">Type of event to subscribe on</typeparam>
    /// <remarks>If you want to subscribe on several events you'll have to </remarks>
    public interface IEventListener<in T> where T : Event
    {
        /// <summary>
        /// Handle event
        /// </summary>
        /// <param name="payload">Event to handle</param>
        /// <remarks>Throwing an event will make the publisher abort the event propagation which means
        /// that any other subscribers will not receive the event.</remarks>
        void Handle(T payload);
    }
}
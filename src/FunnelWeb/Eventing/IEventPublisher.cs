namespace FunnelWeb.Eventing
{
    /// <summary>
    /// Defines contract for the class which will dispatch the events to all subscribers
    /// </summary>
    /// <remarks>How the actual subscription is made is defined by the implementation</remarks>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish a new event to all subscribers
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="payload">Event to publish</param>
        /// <remarks>the publishing will be aborted if any of the subscribers throw an exception.</remarks>
        void Publish<T>(T payload) where T : Event;
    }
}
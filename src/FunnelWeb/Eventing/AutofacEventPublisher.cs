using System;
using System.Collections.Generic;
using Autofac;

namespace FunnelWeb.Eventing
{
    /// <summary>
    /// Uses AutoFac to dispatch the events
    /// </summary>
    /// <remarks>Will dispatch the event to all classes which has been registered in the container and implement <see cref="IEventListener{T}"/></remarks>
    public class AutofacEventPublisher : IEventPublisher
    {
        private readonly IComponentContext container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacEventPublisher" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public AutofacEventPublisher(IComponentContext container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        /// <summary>
        /// Publish a new event to all subscribers
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="payload">Event to publish</param>
        /// <exception cref="System.ArgumentNullException">payload</exception>
        /// <remarks>
        ///   <para>
        /// the publishing will be aborted if any of the subscribers throw an exception.
        ///   </para>
        ///   <para>Will dispatch the event to all classes registered in the container and implementing <see cref="IEventListener{T}" /></para>
        /// </remarks>
        public void Publish<T>(T payload) where T : Event
        {
            if (payload == null) throw new ArgumentNullException("payload");
            foreach (var listener in this.container.Resolve<IEnumerable<IEventListener<T>>>())
            {
                listener.Handle(payload);
            }
        }
    }
}
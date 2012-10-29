using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Eventing
{
    public interface IEventListener
    {
        void Handle(Event payload);
    }

    public interface IEventPublisher
    {
        void Publish(Event payload);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly IEnumerable<IEventListener> eventListeners;

        public EventPublisher(IEnumerable<IEventListener> eventListeners)
        {
            this.eventListeners = eventListeners;
        }

        public void Publish(Event payload)
        {
            foreach (var listener in eventListeners)
            {
                listener.Handle(payload);
            }
        }
    }

    public abstract class Event
    {
        
    }

    public class CommentPostedEvent : Event
    {
        private readonly Entry entry;
        private readonly Comment comment;

        public CommentPostedEvent(Entry entry, Comment comment)
        {
            this.entry = entry;
            this.comment = comment;
        }

        public Entry Entry
        {
            get { return entry; }
        }

        public Comment Comment
        {
            get { return comment; }
        }
    }

    public class EntrySavedEvent : Event
    {
        private readonly Entry entry;

        public EntrySavedEvent(Entry entry)
        {
            this.entry = entry;
        }

        public Entry Entry
        {
            get { return entry; }
        }
    }
}
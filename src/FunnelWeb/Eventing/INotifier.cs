using System.Web.Mvc;
using FunnelWeb.Model;

namespace FunnelWeb.Eventing
{
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
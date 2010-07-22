using System;

namespace FunnelWeb.Web.Model
{
    public class FeedItem
    {
        public FeedItem()
        {
        }

        public virtual int Id { get; private set; }
        public virtual Entry Entry { get; set; }
        public virtual Feed Feed { get; set; }
        public virtual DateTime SortDate { get; set; }
    }
}
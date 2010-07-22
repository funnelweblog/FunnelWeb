using System.Linq;
using Bindable.Core.Helpers;
using Iesi.Collections.Generic;
using FunnelWeb.Web.Model.Strings;
using System;

namespace FunnelWeb.Web.Model
{
    public class Feed
    {
        public Feed()
        {
            Name = string.Empty;
            Title = string.Empty;
            Items = new HashedSet<FeedItem>();
        }

        public virtual int Id { get; private set; }
        public virtual PageName Name { get; set; }
        public virtual string Title { get; set; }
        public virtual ISet<FeedItem> Items { get; private set; }

        public virtual void Publish(Entry entry)
        {
            Guard.NotNull(entry, "entry");
            var newItem = new FeedItem { Entry = entry, Feed = this, SortDate = DateTime.UtcNow };
            var bad = Items.Where(x => x.Entry.Id == entry.Id).ToList();
            Items.RemoveAll(bad);
            Items.Add(newItem);
        }
    }
}
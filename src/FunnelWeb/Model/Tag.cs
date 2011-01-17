using System;
using System.Linq;
using Bindable.Core.Helpers;
using Iesi.Collections.Generic;

namespace FunnelWeb.Model
{
    public class Tag
    {
        public Tag()
        {
            Name = string.Empty;
            Items = new HashedSet<TagItem>();
        }

        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual ISet<TagItem> Items { get; private set; }

        public virtual void Add(Entry entry)
        {
            Guard.NotNull(entry, "entry");
            var existing = Items.FirstOrDefault(x => x.Entry == entry);
            if (existing != null)
                return;

            var newItem = new TagItem { Entry = entry, Tag = this };
            Items.Add(newItem);
        }

        public virtual void Remove(Entry entry)
        {
            Guard.NotNull(entry, "entry");
            var existing = Items.FirstOrDefault(x => x.Entry == entry);
            if (existing != null)
            {
                Items.Remove(existing);
            }
        }
    }
}
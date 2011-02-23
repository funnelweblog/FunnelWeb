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
            Entries = new HashedSet<Entry>();
        }

        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual ISet<Entry> Entries { get; private set; }

        public virtual void Add(Entry entry)
        {
            Guard.NotNull(entry, "entry");
            var existing = Entries.FirstOrDefault(x => x == entry);
            if (existing != null)
                return;

            Entries.Add(entry);
            entry.Tags.Add(this);
        }

        public virtual void Remove(Entry entry)
        {
            Guard.NotNull(entry, "entry");
            var existing = Entries.FirstOrDefault(x => x == entry);
            if (existing != null)
            {
                Entries.Remove(existing);
                existing.Tags.Remove(this);
            }
        }
    }
}
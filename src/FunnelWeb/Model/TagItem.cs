using System;

namespace FunnelWeb.Model
{
    public class TagItem
    {
        public TagItem()
        {
        }

        public virtual int Id { get; private set; }
        public virtual Entry Entry { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
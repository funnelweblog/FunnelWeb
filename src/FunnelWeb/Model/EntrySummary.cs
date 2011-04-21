using System;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Model.Strings;
using Iesi.Collections.Generic;

namespace FunnelWeb.Model
{
    public class EntrySummary
    {
        public virtual PageName Name { get; set; }
        public virtual string Title { get; set; }
        
        [DataType("Markdown")]
        public virtual string Summary { get; set; }
        
        public virtual int CommentCount { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual DateTime Published { get; set; }
        
        [DataType("Tags")]
        public virtual ISet<Tag> Tags { get; set; }

    }
}
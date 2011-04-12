using System;
using System.Linq;
using FunnelWeb.Model.Strings;
using Iesi.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Constraints;

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

    public class Entry : EntrySummary
    {
        public Entry()
        {
            Title = string.Empty;
            Name = string.Empty;
            Summary = string.Empty;
            Published = DateTime.UtcNow;
            Revisions = new HashedSet<Revision>();
            Comments = new HashedSet<Comment>();
            Pingbacks = new HashedSet<Pingback>();
            Tags = new HashedSet<Tag>();
            IsDiscussionEnabled = true;
        }

        public virtual int Id { get; private set; }
		public virtual string PageTemplate { get; set; }

        public virtual bool IsDiscussionEnabled { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual bool HideChrome { get; set; }
        public virtual string Status { get; set; }
        public virtual string Author { get; set; }

        [Valid]
        public virtual Revision LatestRevision { get; set; }

        public virtual ISet<Revision> Revisions { get; private set; }
        public virtual ISet<Comment> Comments { get; set; }
        public virtual ISet<Pingback> Pingbacks { get; set; }
        
        public virtual string TagsCommaSeparated
        {
            get { return string.Join(", ", Tags.Select(x => x.Name)); }
        }

        public virtual Revision Revise()
        {
            var original = LatestRevision;
            var revision = new Revision();
            if (original != null)
            {
                revision.Body = original.Body;
                revision.Reason = original.Reason;
            }
            revision.Entry = this;
            revision.Revised = DateTime.UtcNow;
            LatestRevision = revision;
            Revisions.Add(revision);
            return revision;
        }

        public virtual Comment Comment()
        {
            var comment = new Comment
            {
                Entry = this,
                Posted = DateTime.UtcNow
            };
            Comments.Add(comment);
            return comment;
        }
    }
}

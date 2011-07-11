using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FunnelWeb.Model.Strings;
using NHibernate.Validator.Constraints;

namespace FunnelWeb.Model
{
    public class Entry
    {
        public Entry()
        {
            Title = string.Empty;
            Name = string.Empty;
            Summary = string.Empty;
            MetaDescription = string.Empty;
            MetaTitle = string.Empty;
            Status = string.Empty;
            Author = string.Empty;
            HideChrome = false;
            Published = DateTime.UtcNow;
            PageTemplate = null;
            Revisions = new List<Revision>();
            Comments = new List<Comment>();
            Pingbacks = new List<Pingback>();
            Tags = new List<Tag>();
            IsDiscussionEnabled = true;
        }

        public virtual int Id { get; protected set; }
		public virtual string PageTemplate { get; set; }


        public virtual PageName Name { get; set; }
        public virtual string Title { get; set; }

        [DataType("Markdown")]
        public virtual string Summary { get; set; }

        public virtual int CommentCount { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual DateTime Published { get; set; }

        [DataType("Tags")]
        public virtual IList<Tag> Tags { get; set; }

        public virtual string TagsCommaSeparated
        {
            get { return string.Join(",", Tags.Select(t=>t.Name)); }
            set {}
        }

        public virtual bool IsDiscussionEnabled { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual bool HideChrome { get; set; }
        public virtual string Status { get; set; }
        public virtual string Author { get; set; }

        [Valid]
        public virtual Revision LatestRevision { get; set; }

        public virtual IList<Revision> Revisions { get; protected set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Pingback> Pingbacks { get; set; }

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
            revision.RevisionNumber = Revisions.Count + 1;
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
            CommentCount = Comments.Count;
            return comment;
        }
    }
}

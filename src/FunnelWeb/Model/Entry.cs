using System;
using System.Linq;
using FunnelWeb.Model.Strings;
using Iesi.Collections.Generic;
using NHibernate.Validator.Constraints;
using System.ComponentModel.DataAnnotations;

namespace FunnelWeb.Model
{
    public class Entry
    {
        private Revision revision;

        public Entry()
        {
            Title = string.Empty;
            Name = string.Empty;
            Summary = string.Empty;
            IsVisible = true;
            Published = DateTime.UtcNow;
            Revisions = new HashedSet<Revision>();
            Comments = new HashedSet<Comment>();
            Pingbacks = new HashedSet<Pingback>();
            IsDiscussionEnabled = true;
        }

        public virtual int Id { get; private set; }
        public virtual string Title { get; set; }
        public virtual PageName Name { get; set; }

        [DataType("Markdown")]
        public virtual string Summary { get; set; }

        public virtual bool IsVisible { get; set; }
        public virtual DateTime Published { get; set; }
        public virtual bool IsDiscussionEnabled { get; set; }
        public virtual int CommentCount { get; set; }
        public virtual DateTime FeedDate { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string MetaKeywords { get; set; }
        public virtual string MetaTitle { get; set; }
        
        [Valid]
        public virtual Revision LatestRevision 
        { 
            get
            {
                return revision ?? Revisions.OrderByDescending(x => x.Revised).FirstOrDefault();
            }
            set { revision = value; }
        }

        public virtual ISet<Revision> Revisions { get; private set; }
        public virtual ISet<Comment> Comments { get; set; }
        public virtual ISet<Pingback> Pingbacks { get; set; }
        
        public virtual Revision Revise()
        {
            var original = LatestRevision;
            var revision = new Revision();
            if (original != null)
            {
                revision.Body = original.Body;
                revision.IsVisible = original.IsVisible;
                revision.Reason = original.Reason;
                revision.Tags = original.Tags;
            }
            revision.Entry = this;
            revision.Revised = DateTime.UtcNow;
            this.revision = revision;
            Revisions.Add(revision);
            return revision;
        }

        public virtual Comment Comment()
        {
            var comment = new Comment();
            comment.Entry = this;
            comment.Posted = DateTime.UtcNow;
            Comments.Add(comment);
            return comment;
        }
    }
}

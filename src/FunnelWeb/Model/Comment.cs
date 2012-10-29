using System;

namespace FunnelWeb.Model
{
    public class Comment
    {
        public virtual int Id { get; set; }

        public virtual string Body { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual string AuthorUrl { get; set; }
        public virtual string AuthorEmail { get; set; }
        public virtual string AuthorIp { get; set; }
        public virtual Entry Entry { get; set; }
        public virtual DateTime Posted { get; set; }
        public virtual int Status { get; set; }
        public virtual int EntryRevisionNumber { get; set; }

        public virtual bool IsSpam
        {
            get { return Status == 0; }
            set { Status = value ? 0 : 1; }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using NHibernate.Validator.Constraints;

namespace FunnelWeb.Model
{
    public class Revision
    {
        public Revision()
        {
            Body = string.Empty;
            Reason = string.Empty;
            Revised = DateTime.UtcNow;
            Tags = string.Empty;
            Status = 0;
            Format = Formats.Markdown;
        }

        public virtual int Id { get; private set; }
        
        [DataType("Markdown")]
        [NotNullNotEmpty(Message = "Please provide a body for this wiki entry.")]
        public virtual string Body { get; set; }
        
        public virtual string Reason { get; set; }
        public virtual string Format { get; set; }
        public virtual DateTime Revised { get; set; }
        public virtual int RevisionNumber { get; set; }
        public virtual string Tags { get; set; }
        public virtual int Status { get; set; }
        public virtual Entry Entry { get; set; }
    }
}
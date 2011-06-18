using System;

namespace FunnelWeb.Model
{
    public class Pingback
    {
        public virtual int Id { get; protected set; }
        public virtual Entry Entry { get; set; }
        public virtual string TargetUri { get; set; }
        public virtual string TargetTitle { get; set; }
        public virtual bool IsSpam { get; set; }
        public virtual DateTime Received { get; set; }
    }
}
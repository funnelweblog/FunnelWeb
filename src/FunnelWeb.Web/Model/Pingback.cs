namespace FunnelWeb.Web.Model
{
    public class Pingback
    {
        public Pingback()
        {
        }

        public virtual int Id { get; private set; }
        public virtual Entry Entry { get; set; }
        public virtual string TargetUri { get; set; }
        public virtual string TargetTitle { get; set; }
        public virtual bool IsSpam { get; set; }
    }
}
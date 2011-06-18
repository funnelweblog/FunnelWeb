namespace FunnelWeb.Model
{
    public class Redirect
    {
        public virtual int Id { get; set; }
        public virtual string From { get; set; }
        public virtual string To { get; set; }
    }
}

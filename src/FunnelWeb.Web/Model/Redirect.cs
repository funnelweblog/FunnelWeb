using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model
{
    public class Redirect
    {
        public virtual int Id { get; set; }
        public virtual string From { get; set; }
        public virtual string To { get; set; }
    }
}

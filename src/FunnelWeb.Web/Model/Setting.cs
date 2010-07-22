using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model
{
    public class Setting
    {
        public virtual int Id { get; set; }
        public virtual PageName Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Value { get; set; }
    }
}

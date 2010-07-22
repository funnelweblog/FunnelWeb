using FluentNHibernate.Mapping;

namespace FunnelWeb.Web.Model.Mappings
{
    public class RedirectMapping : ClassMap<Redirect>
    {
        public RedirectMapping()
        {
            Id(x => x.Id);
            Map(x => x.From, "[From]");
            Map(x => x.To, "[To]");
        }
    }
}

using FluentNHibernate.Mapping;

namespace FunnelWeb.Web.Model.Mappings
{
    public class PingbackMapping : ClassMap<Pingback>
    {
        public PingbackMapping()
        {
            Id(x => x.Id);
            Map(x => x.IsSpam);
            Map(x => x.TargetTitle);
            Map(x => x.TargetUri);
            References(x => x.Entry, "EntryId");
        }
    }
}
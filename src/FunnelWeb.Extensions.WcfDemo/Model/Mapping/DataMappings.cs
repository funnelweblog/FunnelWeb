using FluentNHibernate.Mapping;

namespace FunnelWeb.Extensions.WcfDemo.Model.Mapping
{
    public class WcfDemoDataMappings : ClassMap<WcfDemoData>
    {
        public WcfDemoDataMappings()
        {
            Id(x => x.Id);
            Map(x => x.Data);
        }
    }
}

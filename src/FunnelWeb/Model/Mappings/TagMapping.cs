using FluentNHibernate.Mapping;

namespace FunnelWeb.Model.Mappings
{
    public class TagMapping : ClassMap<Tag>
    {
        public TagMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            HasMany(x => x.Items)
                .KeyColumn("TagId")
                .AsSet()
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan();
        }
    }
}

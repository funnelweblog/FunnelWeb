using FluentNHibernate.Mapping;
using FunnelWeb.Web.Model.Mappings.UserTypes;

namespace FunnelWeb.Web.Model.Mappings
{
    public class FeedMapping : ClassMap<Feed>
    {
        public FeedMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name).CustomType<PageNameUserType>();
            Map(x => x.Title);
            HasMany(x => x.Items)
                .KeyColumn("FeedId")
                .AsSet()
                .Inverse()
                .LazyLoad()
                .Cascade.AllDeleteOrphan();
        }
    }
}

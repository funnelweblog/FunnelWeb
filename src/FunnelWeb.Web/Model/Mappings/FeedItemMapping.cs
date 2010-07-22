using FluentNHibernate.Mapping;

namespace FunnelWeb.Web.Model.Mappings
{
    public class FeedItemMapping : ClassMap<FeedItem>
    {
        public FeedItemMapping()
        {
            Id(x => x.Id);
            Map(x => x.SortDate);
            References(x => x.Entry, "ItemId");
            References(x => x.Feed, "FeedId");
        }
    }
}
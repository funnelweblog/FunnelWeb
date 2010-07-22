using FluentNHibernate.Mapping;
using FunnelWeb.Web.Model.Comparers;
using FunnelWeb.Web.Model.Mappings.UserTypes;

namespace FunnelWeb.Web.Model.Mappings
{
    public class EntryMapping : ClassMap<Entry>
    {
        public EntryMapping()
        {
            Id(x => x.Id);
            Map(x => x.IsVisible);
            Map(x => x.Name).CustomType<PageNameUserType>();
            Map(x => x.Published);
            Map(x => x.Summary);
            Map(x => x.Title);
            Map(x => x.IsDiscussionEnabled);
            Map(x => x.MetaDescription);
            Map(x => x.MetaKeywords);
            Map(x => x.MetaTitle);
            Map(x => x.CommentCount).Formula("(SELECT COUNT(*) from Comment where Comment.EntryID = ID and Comment.Status = 1)");
            Map(x => x.FeedDate).Formula("(SELECT ISNULL(MAX(FeedItem.SortDate), Published)  from FeedItem where FeedItem.ItemID = ID)");

            HasMany(x => x.Pingbacks)
                .KeyColumn("EntryId")
                .Inverse()
                .AsSet<PingbackComparer>()
                .LazyLoad()
                .Cascade.All();

            HasMany(x => x.Revisions)
                .KeyColumn("EntryId")
                .Inverse()
                .AsSet<RevisionComparer>()
                .LazyLoad()
                .Cascade.All();

            HasMany(x => x.Comments)
                .KeyColumn("EntryId")
                .Inverse()
                .AsSet<CommentComparer>()
                .LazyLoad()
                .Cascade.All();
        }
    }
}

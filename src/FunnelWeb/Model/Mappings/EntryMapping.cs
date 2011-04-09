using FluentNHibernate.Mapping;
using FunnelWeb.Model.Comparers;
using FunnelWeb.Model.Mappings.UserTypes;

namespace FunnelWeb.Model.Mappings
{
    public class EntryMapping : ClassMap<Entry>
    {
        public EntryMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name).CustomType<PageNameUserType>();
            Map(x => x.Published);
            Map(x => x.Summary);
            Map(x => x.Title);
            Map(x => x.IsDiscussionEnabled);
            Map(x => x.MetaDescription);
            Map(x => x.MetaTitle);
            Map(x => x.HideChrome);
            Map(x => x.Status);
			Map(x => x.PageTemplate);
            Map(x => x.Author);
            Map(x => x.CommentCount).Formula("(SELECT COUNT(*) from Comment where Comment.EntryID = ID and Comment.Status = 1)");

            Component(o => o.LatestRevision,
                c=>
                    {
                        c.Map(r => r.Id, "LatestRevisionId");
                        c.Map(r => r.RevisionNumber);
                        c.Map(r => r.Author).Column("RevisionAuthor");
                        c.Map(r => r.Body).Length(int.MaxValue);
                    });

            HasManyToMany(x => x.Tags)
                .Table("TagItem")
                .ParentKeyColumn("EntryId")
                .ChildKeyColumn("TagId")
                .AsSet();

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

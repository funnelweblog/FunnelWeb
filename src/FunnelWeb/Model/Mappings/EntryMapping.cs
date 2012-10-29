using FluentNHibernate.Mapping;
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
            Map(x => x.CommentCount);//.Formula(
            //    string.Format("(SELECT COUNT(*) from {0}{1}Comment where Comment.EntryID = ID and Comment.Status = 1)",
            //    CurrentSchema, string.IsNullOrEmpty(CurrentSchema) ? string.Empty : "."));
            Map(x => x.TagsCommaSeparated);

            Component(o => o.LatestRevision,
                c=>
                    {
                        c.Map(r => r.Id, "LatestRevisionId");
                        c.Map(r => r.RevisionNumber);
                        c.Map(r => r.Author).Column("RevisionAuthor");
                        c.Map(r => r.Body).Column("Body").Length(int.MaxValue);
                        c.Map(r => r.Revised).Column("LastRevised");
                        c.Map(r => r.Format).Column("LatestRevisionFormat");
                    });

            HasManyToMany(x => x.Tags)
                .Table("TagItem")
                .ParentKeyColumn("EntryId")
                .ChildKeyColumn("TagId");

            HasMany(x => x.Pingbacks)
                .KeyColumn("EntryId")
                .Inverse()
                .LazyLoad()
                .Cascade.All();

            HasMany(x => x.Revisions)
                .KeyColumn("EntryId")
                .Inverse()
                .LazyLoad()
                .ApplyFilter<RevisionFilter>("RevisionNumber = :revisionNumber")
                .Cascade.All();

            HasMany(x => x.Comments)
                .KeyColumn("EntryId")
                .Inverse()
                .LazyLoad()
                .Cascade.All();
        }

        public static string CurrentSchema { get; set; }
    }
}

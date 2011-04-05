using FluentNHibernate.Mapping;

namespace FunnelWeb.Model.Mappings
{
    public class CommentMapping : ClassMap<Comment>
    {
        public CommentMapping()
        {
            Id(x => x.Id);
            Map(x => x.AuthorEmail);
            Map(x => x.AuthorName);
            Map(x => x.AuthorUrl);
            Map(x => x.Body);
            Map(x => x.Status);
            Map(x => x.Posted);
            Map(x => x.AuthorIp);
            Map(x => x.EntryRevisionNumber);

            References(x => x.Entry, "EntryId");
        }
    }
}

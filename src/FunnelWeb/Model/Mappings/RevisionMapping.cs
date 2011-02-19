using FluentNHibernate.Mapping;

namespace FunnelWeb.Model.Mappings
{
    public class RevisionMapping : ClassMap<Revision>
    {
        public RevisionMapping()
        {
            Id(x => x.Id);
            Map(x => x.Body).Length(int.MaxValue);
            Map(x => x.Reason);
            Map(x => x.Revised);
            Map(x => x.RevisionNumber);
            Map(x => x.Status);
            Map(x => x.Format);
            References(x => x.Entry, "EntryId");
        }
    }
}
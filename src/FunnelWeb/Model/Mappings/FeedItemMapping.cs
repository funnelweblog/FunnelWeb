using FluentNHibernate.Mapping;

namespace FunnelWeb.Model.Mappings
{
    public class TagItemMapping : ClassMap<TagItem>
    {
        public TagItemMapping()
        {
            Id(x => x.Id);
            References(x => x.Entry, "EntryId");
            References(x => x.Tag, "TagId");
        }
    }
}
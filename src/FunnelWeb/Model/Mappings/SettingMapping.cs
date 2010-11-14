using FluentNHibernate.Mapping;
using FunnelWeb.Model.Mappings.UserTypes;

namespace FunnelWeb.Model.Mappings
{
    public class SettingMapping : ClassMap<Setting>
    {
        public SettingMapping()
        {
            Id(x => x.Id);
            Map(x => x.Description);
            Map(x => x.DisplayName);
            Map(x => x.Name).CustomType<PageNameUserType>();
            Map(x => x.Value);
        }
    }
}

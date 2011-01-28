using FluentNHibernate.Mapping;

namespace FunnelWeb.Extensions.SqlAuthentication.Model.Mappings
{
    public class RoleMapping : ClassMap<Role>
    {
        public RoleMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}

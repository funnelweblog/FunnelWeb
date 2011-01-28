using FluentNHibernate.Mapping;

namespace FunnelWeb.Extensions.SqlAuthentication.Model.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Username);
            Map(x => x.Email);
            Map(x => x.Password);
            HasManyToMany(x => x.Roles)
                .Table("UserRoles")
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId");
        }
    }
}

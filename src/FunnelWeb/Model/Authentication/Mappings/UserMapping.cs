using System;
using FluentNHibernate.Mapping;

namespace FunnelWeb.Model.Authentication.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Username);
            Map(x => x.Email);
            Map(x => x.Password);
            HasManyToMany(x => x.Roles)
                .AsBag()
                .Table("UserRoles")
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("RoleId")
                .Inverse()
                .Cascade.None();
        }
    }
}

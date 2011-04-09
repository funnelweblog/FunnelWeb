using System.Collections.Generic;

namespace FunnelWeb.Extensions.SqlAuthentication.Model
{
    public class AddRoleModel
    {
        public User User { get; set; }
        public IList<Role> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Web.Areas.Admin.Views.SqlAuthentication
{
    public class AddRoleModel
    {
        public User User { get; set; }
        public IList<Role> Roles { get; set; }
    }
}

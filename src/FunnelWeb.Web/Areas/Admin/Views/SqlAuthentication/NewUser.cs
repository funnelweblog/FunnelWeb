using System;
using FunnelWeb.Model.Authentication;

namespace FunnelWeb.Web.Areas.Admin.Views.SqlAuthentication
{
    public class NewUser : User
    {
        public virtual string RepeatPassword { get; set; }
    }
}
using System;
using FunnelWeb.Model.Authentication;
using System.ComponentModel;

namespace FunnelWeb.Web.Areas.Admin.Views.SqlAuthentication
{
    public class NewUser : User
    {
        [DisplayName("Repeat Password")]
        public virtual string RepeatPassword { get; set; }
    }
}
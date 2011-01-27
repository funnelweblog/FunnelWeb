using System.ComponentModel.DataAnnotations;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.Areas.Admin.Views.Login
{
    public class LoginModel
    {
        public LoginModel()
        {
        }

        public bool? DatabaseIssue { get; set; }
        public string ReturnUrl { get; set; }

        [Required]
        [StringLength(100)]
        [HintSize(HintSize.Medium)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [HintSize(HintSize.Medium)]
        public string Password { get; set; }
    }
}
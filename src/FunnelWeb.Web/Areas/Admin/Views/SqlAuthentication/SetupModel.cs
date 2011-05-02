using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FunnelWeb.Mvc;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web.Areas.Admin.Views.SqlAuthentication
{
    public class SetupModel
    {
        public bool HasAdminAccount { get; set; }

        [Required]
        [StringLength(100)]
        [HintSize(HintSize.Medium)]
        public string Name { get; set; }

        [DisplayName("E-mail")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [HintSize(HintSize.Medium)]
        [RegularExpression("^[A-Za-z0-9._%+-]+@([A-Za-z0-9-]+\\.)+([A-Za-z0-9]{2,4}|museum)$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [HintSize(HintSize.Medium)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [HintSize(HintSize.Medium)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [HintSize(HintSize.Medium)]
        [DataType(DataType.Password)]
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }
    }
}
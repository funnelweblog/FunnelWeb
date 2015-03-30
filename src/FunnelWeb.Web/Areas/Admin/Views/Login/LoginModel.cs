using System.ComponentModel.DataAnnotations;
using FunnelWeb.Mvc;

namespace FunnelWeb.Web.Areas.Admin.Views.Login
{
	public class LoginModel
	{
		public bool? DatabaseIssue { get; set; }
		public bool? ConfigFileMissing { get; set; }
		public string ReturnUrl { get; set; }
		public bool DatabaseConnectionIssue { get; set; }
		public string DatabaseError { get; set; }

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
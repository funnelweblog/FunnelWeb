using System.IdentityModel.Services;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Controllers
{
	public class LoginController : Controller
	{
		public IFederatedAuthenticationConfigurator FederatedAuthenticationConfigurator { get; set; }

		public ActionResult Index()
		{
			var returnUrl = Request.QueryString["ReturnUrl"];

			ViewBag.MetaDataScript = FederatedAuthenticationConfigurator.GenerateMetadataScript(returnUrl);

			return View();
		}

		/// <summary>
		/// If we return here "~/login/return" after authenticating in the ACS we can try to fish out the ReturnUrl from the posted data and return to the correct page.
		/// </summary>
		[HttpPost]
		public ActionResult Return()
		{
			if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index");
			}

			var formData = ControllerContext.HttpContext.Request.Unvalidated.Form;

			string contextQuery = null;

			if (formData["wresult"] != null)
			{
				var baseUrl = FederationMessage.GetBaseUrl(ControllerContext.HttpContext.Request.Url);
				var signInResponseMessage = WSFederationMessage.CreateFromNameValueCollection(baseUrl, formData) as SignInResponseMessage;

				if (signInResponseMessage != null)
				{
					contextQuery = signInResponseMessage.Context;
				}
			}

			var returnUrl = GetReturnUrl(contextQuery);

			return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl);
		}

		/// <summary>
		/// Given a context query from ACS try to find a ReturnUrl parameter and return it's value.
		/// </summary>
		public static string GetReturnUrl(string contextQuery)
		{
			if (string.IsNullOrWhiteSpace(contextQuery))
			{
				return null;
			}

			// Decode the context.
			var urlDecode = HttpUtility.UrlDecode(contextQuery);

			if (string.IsNullOrWhiteSpace(urlDecode))
			{
				return null;
			}

			var returnUrl = urlDecode
				.Split('&') // There may be many parts in the context
				.Select(val =>
				{
					var split = val.Split('=');
					return new { Key = split[0], Value = split[1] };
				}) // Each part is on the querystring format <key>=<value>
				.SingleOrDefault(d => d.Key == "ReturnUrl"); // We want the one named "ReturnUrl"

			return returnUrl != null ? returnUrl.Value : null;
		}
	}
}
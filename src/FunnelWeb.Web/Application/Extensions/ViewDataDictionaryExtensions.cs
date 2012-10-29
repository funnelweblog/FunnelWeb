using System.Web;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public static bool IsLoggedIn(this ViewDataDictionary viewData)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}

using System.Web.Mvc;
using System.Web;

namespace FunnelWeb.Web.Application.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public static void Flash(this ViewDataDictionary viewData, string messageFormat, params object[] formatArguments)
        {
            if (!viewData.ContainsKey("FlashData"))
                viewData.Add("FlashData", new FlashData());
            ((FlashData) viewData["FlashData"]).Add(messageFormat, formatArguments);
        }

        public static bool IsLoggedIn(this ViewDataDictionary viewData)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}

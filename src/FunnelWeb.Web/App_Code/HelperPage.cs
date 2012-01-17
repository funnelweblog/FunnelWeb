using System;
using System.Web.Mvc;
using System.Web.WebPages;

namespace FunnelWeb.Web
{
    public class HelperPage : System.Web.WebPages.HelperPage 
    {
        // Workaround - exposes the MVC HtmlHelper instead of the crappy System.Web.WebPages HtmlHelper
        public static new HtmlHelper Html
        {
            get { return ((System.Web.Mvc.WebViewPage) WebPageContext.Current.Page).Html; }
        }

        public static new UrlHelper Url
        {
            get { return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Url; }
        }

        public static ViewDataDictionary ViewData
        {
            get { return ((System.Web.Mvc.WebViewPage) WebPageContext.Current.Page).ViewData; }
        }
    }
}
using System;
using System.Web;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class UrlHelperExtension
    {
        public static string Absolute(this UrlHelper url, string relativeOrAbsolute)
        {
            var uri = new Uri(relativeOrAbsolute, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                return relativeOrAbsolute;
            }
            // At this point, we know the url is relative.
            string absolute = VirtualPathUtility.ToAbsolute(relativeOrAbsolute);
            return absolute;
        }
    }
}
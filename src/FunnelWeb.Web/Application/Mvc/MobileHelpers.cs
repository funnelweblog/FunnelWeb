using System;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class MobileHelpers
    {
        public static bool UserAgentContains(this ControllerContext c, string agentToFind)
        {
            return (c.HttpContext.Request.UserAgent.IndexOf(agentToFind, StringComparison.OrdinalIgnoreCase) > 0);
        }

        public static bool IsMobileDevice(this ControllerContext c)
        {
            return c.HttpContext.Request.Browser.IsMobileDevice;
        }

        public static void AddGenericMobile<T>(this ViewEngineCollection ves)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(
                c => c.IsMobileDevice()
                || c.UserAgentContains("Android")                
                || c.UserAgentContains("iPhone")
                || c.UserAgentContains("Windows Phone"),
                "Mobile", new T()));
        }
    }
}
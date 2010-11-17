using System;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Mvc
{
    /// <remarks>
    /// Thanks Scott! http://www.hanselman.com/blog/ABetterASPNETMVCMobileDeviceCapabilitiesViewEngine.aspx
    /// </remarks>
    public class CustomMobileViewEngine : IViewEngine
    {
        public CustomMobileViewEngine(Func<ControllerContext, bool> isTheRightDevice, string pathToSearch, IViewEngine baseViewEngine)
        {
            BaseViewEngine = baseViewEngine;
            IsTheRightDevice = isTheRightDevice;
            PathToSearch = pathToSearch;
        }

        public IViewEngine BaseViewEngine { get; private set; }
        public Func<ControllerContext, bool> IsTheRightDevice { get; private set; }
        public string PathToSearch { get; private set; }

        public ViewEngineResult FindPartialView(ControllerContext context, string viewName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindPartialView(context, PathToSearch + "/" + viewName, useCache);
            }
            return new ViewEngineResult(new string[] { });
        }

        public ViewEngineResult FindView(ControllerContext context, string viewName, string masterName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindView(context, PathToSearch + "/" + viewName, masterName, useCache);
            }
            return new ViewEngineResult(new string[] { });
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }
    }
}
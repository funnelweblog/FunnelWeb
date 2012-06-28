using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace FunnelWeb.Tests.Web.Application.Extensions
{
    public class HtmlHelperBuilder
    {
        public static HtmlHelper GetHtmlHelper(bool clientValidationEnabled = true)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(Substitute.For<IViewEngine>());

            var controller = Substitute.For<ControllerBase>();
            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.Headers.Returns(new NameValueCollection());

            var routeData = new RouteData();
            routeData.Values["controller"] = "home";
            routeData.Values["action"] = "index";

            var controllerContext = new ControllerContext(httpContext, routeData, controller);

            var viewContext = new ViewContext(controllerContext, Substitute.For<IView>(), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter())
                                  {
                                      HttpContext = httpContext,
                                      ClientValidationEnabled = clientValidationEnabled,
                                      UnobtrusiveJavaScriptEnabled = clientValidationEnabled,
                                      FormContext = new FormContext()
                                  };

            return new HtmlHelper(viewContext, Substitute.For<IViewDataContainer>());
        }
    }
}
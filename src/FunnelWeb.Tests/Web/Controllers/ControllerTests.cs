using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Repositories;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Markup;
using FunnelWeb.Web.Application.Markup.Macros;
using NSubstitute;

namespace FunnelWeb.Tests.Web.Controllers
{
    public class ControllerTests
    {
        protected IIdentity Identity { get; set; }
        protected IPrincipal User { get; set; }
        protected ISettingsProvider SettingsProvider { get; set; }
        protected ControllerContext ControllerContext { get; set; }
        protected UrlHelper UrlHelper { get; set; }
        protected IRepository Repository { get; set; }
        protected IContentRenderer ContentRenderer { get; set; }
        protected HtmlHelper Html { get; set; }

        public ControllerTests()
        {
            Identity = Substitute.For<IIdentity>();
            User = Substitute.For<IPrincipal>();
            Repository = Substitute.For<IRepository>();
            SettingsProvider = Substitute.For<ISettingsProvider>();
            ContentRenderer = Substitute.For<IContentRenderer>();
            ControllerContext = CreateControllerContext();
            UrlHelper = CreateUrlHelper();
            User.Identity.Returns(Identity);
            ControllerContext.HttpContext.User.Returns(User);
            SettingsProvider.GetSettings<FunnelWebSettings>().Returns(new FunnelWebSettings { EnablePublicHistory = true });
            Html = CreateHelper();
            ContentRenderer.RenderTrusted(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<HtmlHelper>()).Returns(c => c.Args()[0]);
            ContentRenderer.RenderUntrusted(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<HtmlHelper>()).Returns(c => c.Args()[0]);
        }

        private UrlHelper CreateUrlHelper()
        {
            return new UrlHelper(ControllerContext.RequestContext);
        }

        private HtmlHelper CreateHelper()
        {
            return new HtmlHelper(new ViewContext(ControllerContext, new DummyView(), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new CustomViewDataContainer());
        }

        private static ControllerContext CreateControllerContext()
        {
            string host = "www.google.com";
            string proto = "http";
            string userIP = "127.0.0.1";

            var headers = new System.Collections.Specialized.NameValueCollection {
                {"Host", host},
                {"X-Forwarded-Proto", proto},
                {"X-Forwarded-For", userIP}
            };

            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri(proto + "://" + host));
            httpRequest.Headers.Returns(headers);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.Returns(httpRequest);

            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext;

            return controllerContext;
        }

        internal class DummyView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {

            }
        }
    }
}
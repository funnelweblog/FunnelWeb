using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FunnelWeb.Routing
{
	public class FunnelWebMvcHandler : MvcHandler
	{
		public FunnelWebMvcHandler(RequestContext requestContext) : base(requestContext)
		{
			requestContext.HttpContext = new HttpContextDecorator(requestContext.HttpContext);
		}

		protected override IAsyncResult BeginProcessRequest(HttpContextBase httpContext, AsyncCallback callback, object state)
		{
			httpContext = new HttpContextDecorator(httpContext);
			return base.BeginProcessRequest(httpContext, callback, state);
		}

		protected override void ProcessRequest(HttpContextBase httpContext)
		{
			httpContext = new HttpContextDecorator(httpContext);
			base.ProcessRequest(httpContext);
		}
	}
}

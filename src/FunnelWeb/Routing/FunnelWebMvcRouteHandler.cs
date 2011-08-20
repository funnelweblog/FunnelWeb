using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FunnelWeb.Routing
{
	public class FunnelWebMvcRouteHandler : MvcRouteHandler
	{
		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new FunnelWebMvcHandler(requestContext);
		}
	}
}

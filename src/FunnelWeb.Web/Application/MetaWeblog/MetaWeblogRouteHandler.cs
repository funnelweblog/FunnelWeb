using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FunnelWeb.Web.Application.MetaWeblog
{
    public class MetaWeblogRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return DependencyResolver.Current.GetService<IMetaWeblog>();
        }
    }
}

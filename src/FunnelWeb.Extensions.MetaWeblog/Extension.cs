using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Extensions.MetaWeblog
{
    [FunnelWebExtension(FullName = "Support for MetaWeblog  API", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
	public class Extension : RoutableFunnelWebExtension
    {
		public override void Initialize(ContainerBuilder builder)
		{
			builder.RegisterType<MetaWeblog>().As<IMetaWeblog>().InstancePerLifetimeScope();
			// http://www.cookcomputing.com/blog/archives/xml-rpc-and-asp-net-mvc
			Routes.MapRoute(null, "wlwmanifest.xml", new {controller = "MetaWeblog", action = "WlwManifest"});
			Routes.Add(new Route("{weblog}", null, new RouteValueDictionary(new {weblog = "blogapi"}), new MetaWeblogRouteHandler()));
		}
    }
}
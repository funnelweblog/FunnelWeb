using System.Reflection;
using System.Web;
using FunnelWeb.Web;

[assembly: AssemblyTitle("FunnelWeb.Web")]
[assembly: AssemblyDescription("ASP.NET MVC assembly for FunnelWeb")]

[assembly: PreApplicationStartMethod(
  typeof(MvcApplication), "Initialise")]
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using FunnelWeb.Web;

[assembly: AssemblyTitle("FunnelWeb.Web")]
[assembly: AssemblyDescription("ASP.NET MVC assembly for FunnelWeb")]

[assembly: PreApplicationStartMethod(typeof(MvcApplication), "BeforeApplicationStart")]

[assembly: InternalsVisibleTo("FunnelWeb.Tests")]
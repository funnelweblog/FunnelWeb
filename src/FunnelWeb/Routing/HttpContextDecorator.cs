using System.Web;

namespace FunnelWeb.Routing
{
	public class HttpContextDecorator : HttpContextProxy
	{
		public HttpContextDecorator(HttpContextBase innerHttpContext) : base(innerHttpContext)
		{
		}

		public override HttpRequestBase Request
		{
			get
			{
				return new HttpRequestDecorator(base.Request);
			}
		}
	}
}

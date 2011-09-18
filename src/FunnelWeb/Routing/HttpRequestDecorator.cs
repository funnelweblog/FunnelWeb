using System;
using System.Web;

namespace FunnelWeb.Routing
{
	public class HttpRequestDecorator : HttpRequestProxy
	{
		public HttpRequestDecorator(HttpRequestBase innerHttpRequest) : base(innerHttpRequest)
		{
		}

		public override bool IsSecureConnection
		{
			get
			{
				return String.Equals(Headers["X-Forwarded-Proto"], "https", StringComparison.OrdinalIgnoreCase);
			}
		}

		public override Uri Url
		{
			get
			{
				Uri url = base.Url;
				UriBuilder urlBuilder = new UriBuilder(url);

				if (IsLocal)
				{
					// Do nothing
					// When we're running the application from localhost (e.g. debugging from Visual Studio), we still need the port number
				}
				else if (IsSecureConnection)
				{
					urlBuilder.Port = 443;
					urlBuilder.Scheme = "https";
				}
				else
				{
					urlBuilder.Port = 80;
				}

				return urlBuilder.Uri;
			}
		}

		public override string UserHostAddress
		{
			get
			{
				var forwardedFor = ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (forwardedFor != null)
				{
					return forwardedFor;
				}

				return base.UserHostAddress;
			}
		}
	}
}

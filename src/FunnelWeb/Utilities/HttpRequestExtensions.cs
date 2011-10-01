using System;
using System.Web;

namespace FunnelWeb.Utilities
{
	public static class HttpRequestExtensions
	{
		public static Uri GetOriginalUrl(this HttpRequest request)
		{
			return GetOriginalUrl(new HttpRequestWrapper(request));
		}

		public static Uri GetOriginalUrl(this HttpRequestBase request)
		{
			UriBuilder hostUrl = new UriBuilder();
			string hostHeader = request.Headers["Host"];

			if (hostHeader.Contains(":"))
			{
				hostUrl.Host = hostHeader.Split(':')[0];
				hostUrl.Port = Convert.ToInt32(hostHeader.Split(':')[1]);
			}
			else
			{
				hostUrl.Host = hostHeader;
				hostUrl.Port = -1;
			}

			Uri url = request.Url;
			UriBuilder uriBuilder = new UriBuilder(url);

			if (String.Equals(hostUrl.Host, "localhost", StringComparison.OrdinalIgnoreCase))
			{
				// Do nothing
				// When we're running the application from localhost (e.g. debugging from Visual Studio), we still need the port number
				return uriBuilder.Uri;
			}

			bool isSecureConnection = String.Equals(request.Headers["X-Forwarded-Proto"], "https", StringComparison.OrdinalIgnoreCase);

			if (isSecureConnection)
			{
				uriBuilder.Port = hostUrl.Port == -1 ? 443 : hostUrl.Port;
				uriBuilder.Scheme = "https";
			}
			else
			{
				uriBuilder.Port = hostUrl.Port == -1 ? 80 : hostUrl.Port;
				uriBuilder.Scheme = "http";
			}

			uriBuilder.Host = hostUrl.Host;

			return uriBuilder.Uri;
		}

		public static string GetOriginalUserHostAddress(this HttpRequest request)
		{
			return GetOriginalUserHostAddress(new HttpRequestWrapper(request));
		}

		public static string GetOriginalUserHostAddress(this HttpRequestBase request)
		{
			var forwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (String.IsNullOrEmpty(forwardedFor))
			{
				return request.UserHostAddress;
			}

			return forwardedFor;
		}
	}
}

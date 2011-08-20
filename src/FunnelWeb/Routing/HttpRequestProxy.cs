using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace FunnelWeb.Routing
{
	public abstract class HttpRequestProxy : HttpRequestBase
	{
		private readonly HttpRequestBase innerHttpRequest;

		public HttpRequestProxy(HttpRequestBase innerHttpRequest)
		{
			this.innerHttpRequest = innerHttpRequest;
		}

		public override string[] AcceptTypes
		{
			get
			{
				return innerHttpRequest.AcceptTypes;
			}
		}

		public override string AnonymousID
		{
			get
			{
				return innerHttpRequest.AnonymousID;
			}
		}

		public override string ApplicationPath
		{
			get
			{
				return innerHttpRequest.ApplicationPath;
			}
		}

		public override string AppRelativeCurrentExecutionFilePath
		{
			get
			{
				return innerHttpRequest.AppRelativeCurrentExecutionFilePath;
			}
		}

		public override byte[] BinaryRead(int count)
		{
			return innerHttpRequest.BinaryRead(count);
		}

		public override HttpBrowserCapabilitiesBase Browser
		{
			get
			{
				return innerHttpRequest.Browser;
			}
		}

		public override HttpClientCertificate ClientCertificate
		{
			get
			{
				return innerHttpRequest.ClientCertificate;
			}
		}

		public override Encoding ContentEncoding
		{
			get
			{
				return innerHttpRequest.ContentEncoding;
			}
			set
			{
				innerHttpRequest.ContentEncoding = value;
			}
		}

		public override int ContentLength
		{
			get
			{
				return innerHttpRequest.ContentLength;
			}
		}

		public override string ContentType
		{
			get
			{
				return innerHttpRequest.ContentType;
			}
			set
			{
				innerHttpRequest.ContentType = value;
			}
		}

		public override HttpCookieCollection Cookies
		{
			get
			{
				return innerHttpRequest.Cookies;
			}
		}

		public override string CurrentExecutionFilePath
		{
			get
			{
				return innerHttpRequest.CurrentExecutionFilePath;
			}
		}

		public override string FilePath
		{
			get
			{
				return innerHttpRequest.FilePath;
			}
		}

		public override HttpFileCollectionBase Files
		{
			get
			{
				return innerHttpRequest.Files;
			}
		}

		public override Stream Filter
		{
			get
			{
				return innerHttpRequest.Filter;
			}
			set
			{
				innerHttpRequest.Filter = value;
			}
		}

		public override NameValueCollection Form
		{
			get
			{
				return innerHttpRequest.Form;
			}
		}

		public override NameValueCollection Headers
		{
			get
			{
				return innerHttpRequest.Headers;
			}
		}

		public override ChannelBinding HttpChannelBinding
		{
			get
			{
				return innerHttpRequest.HttpChannelBinding;
			}
		}

		public override string HttpMethod
		{
			get
			{
				return innerHttpRequest.HttpMethod;
			}
		}

		public override Stream InputStream
		{
			get
			{
				return innerHttpRequest.InputStream;
			}
		}

		public override bool IsAuthenticated
		{
			get
			{
				return innerHttpRequest.IsAuthenticated;
			}
		}

		public override bool IsLocal
		{
			get
			{
				return innerHttpRequest.IsLocal;
			}
		}

		public override bool IsSecureConnection
		{
			get
			{
				return innerHttpRequest.IsSecureConnection;
			}
		}

		public override WindowsIdentity LogonUserIdentity
		{
			get
			{
				return innerHttpRequest.LogonUserIdentity;
			}
		}

		public override int[] MapImageCoordinates(string imageFieldName)
		{
			return innerHttpRequest.MapImageCoordinates(imageFieldName);
		}

		public override string MapPath(string virtualPath)
		{
			return innerHttpRequest.MapPath(virtualPath);
		}

		public override string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
		{
			return innerHttpRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping);
		}

		public override NameValueCollection Params
		{
			get
			{
				return innerHttpRequest.Params;
			}
		}

		public override string Path
		{
			get
			{
				return innerHttpRequest.Path;
			}
		}

		public override string PathInfo
		{
			get
			{
				return innerHttpRequest.PathInfo;
			}
		}

		public override string PhysicalApplicationPath
		{
			get
			{
				return innerHttpRequest.PhysicalApplicationPath;
			}
		}

		public override string PhysicalPath
		{
			get
			{
				return innerHttpRequest.PhysicalPath;
			}
		}

		public override NameValueCollection QueryString
		{
			get
			{
				return innerHttpRequest.QueryString;
			}
		}

		public override string RawUrl
		{
			get
			{
				return innerHttpRequest.RawUrl;
			}
		}

		public override RequestContext RequestContext
		{
			get
			{
				return innerHttpRequest.RequestContext;
			}
		}

		public override string RequestType
		{
			get
			{
				return innerHttpRequest.RequestType;
			}
			set
			{
				innerHttpRequest.RequestType = value;
			}
		}

		public override void SaveAs(string filename, bool includeHeaders)
		{
			innerHttpRequest.SaveAs(filename, includeHeaders);
		}

		public override NameValueCollection ServerVariables
		{
			get
			{
				return innerHttpRequest.ServerVariables;
			}
		}

		public override string this[string key]
		{
			get
			{
				return innerHttpRequest[key];
			}
		}

		public override int TotalBytes
		{
			get
			{
				return innerHttpRequest.TotalBytes;
			}
		}

		public override Uri Url
		{
			get
			{
				return innerHttpRequest.Url;
			}
		}

		public override Uri UrlReferrer
		{
			get
			{
				return innerHttpRequest.UrlReferrer;
			}
		}

		public override string UserAgent
		{
			get
			{
				return innerHttpRequest.UserAgent;
			}
		}

		public override string UserHostAddress
		{
			get
			{
				return innerHttpRequest.UserHostAddress;
			}
		}

		public override string UserHostName
		{
			get
			{
				return innerHttpRequest.UserHostName;
			}
		}

		public override string[] UserLanguages
		{
			get
			{
				return innerHttpRequest.UserLanguages;
			}
		}

		public override void ValidateInput()
		{
			innerHttpRequest.ValidateInput();
		}
	}
}

using System;
using System.Collections;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;

namespace FunnelWeb.Routing
{
	public abstract class HttpContextProxy : HttpContextBase
	{
		private readonly HttpContextBase innerHttpContext;

		protected HttpContextProxy(HttpContextBase innerHttpContext)
		{
			this.innerHttpContext = innerHttpContext;
		}

		public override void AddError(Exception errorInfo)
		{
			innerHttpContext.AddError(errorInfo);
		}

		public override Exception[] AllErrors
		{
			get
			{
				return innerHttpContext.AllErrors;
			}
		}

		public override HttpApplicationStateBase Application
		{
			get
			{
				return innerHttpContext.Application;
			}
		}

		public override HttpApplication ApplicationInstance
		{
			get
			{
				return innerHttpContext.ApplicationInstance;
			}
			set
			{
				innerHttpContext.ApplicationInstance = value;
			}
		}

		public override Cache Cache
		{
			get
			{
				return innerHttpContext.Cache;
			}
		}

		public override void ClearError()
		{
			innerHttpContext.ClearError();
		}

		public override IHttpHandler CurrentHandler
		{
			get
			{
				return innerHttpContext.CurrentHandler;
			}
		}

		public override RequestNotification CurrentNotification
		{
			get
			{
				return innerHttpContext.CurrentNotification;
			}
		}

		public override Exception Error
		{
			get
			{
				return innerHttpContext.Error;
			}
		}

		public override object GetGlobalResourceObject(string classKey, string resourceKey)
		{
			return innerHttpContext.GetGlobalResourceObject(classKey, resourceKey);
		}

		public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
		{
			return innerHttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
		}

		public override object GetLocalResourceObject(string virtualPath, string resourceKey)
		{
			return innerHttpContext.GetLocalResourceObject(virtualPath, resourceKey);
		}

		public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
		{
			return innerHttpContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
		}

		public override object GetSection(string sectionName)
		{
			return innerHttpContext.GetSection(sectionName);
		}

		public override object GetService(Type serviceType)
		{
			return innerHttpContext.GetService(serviceType);
		}

		public override IHttpHandler Handler
		{
			get
			{
				return innerHttpContext.Handler;
			}
			set
			{
				innerHttpContext.Handler = value;
			}
		}

		public override bool IsCustomErrorEnabled
		{
			get
			{
				return innerHttpContext.IsCustomErrorEnabled;
			}
		}

		public override bool IsDebuggingEnabled
		{
			get
			{
				return innerHttpContext.IsDebuggingEnabled;
			}
		}

		public override bool IsPostNotification
		{
			get
			{
				return innerHttpContext.IsPostNotification;
			}
		}

		public override IDictionary Items
		{
			get
			{
				return innerHttpContext.Items;
			}
		}

		public override IHttpHandler PreviousHandler
		{
			get
			{
				return innerHttpContext.PreviousHandler;
			}
		}

		public override ProfileBase Profile
		{
			get
			{
				return innerHttpContext.Profile;
			}
		}

		public override void RemapHandler(IHttpHandler handler)
		{
			innerHttpContext.RemapHandler(handler);
		}

		public override HttpRequestBase Request
		{
			get
			{
				return innerHttpContext.Request;
			}
		}

		public override HttpResponseBase Response
		{
			get
			{
				return innerHttpContext.Response;
			}
		}

		public override void RewritePath(string filePath, string pathInfo, string queryString)
		{
			innerHttpContext.RewritePath(filePath, pathInfo, queryString);
		}

		public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
		{
			innerHttpContext.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
		}

		public override void RewritePath(string path)
		{
			innerHttpContext.RewritePath(path);
		}

		public override void RewritePath(string path, bool rebaseClientPath)
		{
			innerHttpContext.RewritePath(path, rebaseClientPath);
		}

		public override HttpServerUtilityBase Server
		{
			get
			{
				return innerHttpContext.Server;
			}
		}

		public override HttpSessionStateBase Session
		{
			get
			{
				return innerHttpContext.Session;
			}
		}

		public override void SetSessionStateBehavior(SessionStateBehavior sessionStateBehavior)
		{
			innerHttpContext.SetSessionStateBehavior(sessionStateBehavior);
		}

		public override bool SkipAuthorization
		{
			get
			{
				return innerHttpContext.SkipAuthorization;
			}
			set
			{
				innerHttpContext.SkipAuthorization = value;
			}
		}

		public override DateTime Timestamp
		{
			get
			{
				return innerHttpContext.Timestamp;
			}
		}

		public override TraceContext Trace
		{
			get
			{
				return innerHttpContext.Trace;
			}
		}

		public override IPrincipal User
		{
			get
			{
				return innerHttpContext.User;
			}
			set
			{
				innerHttpContext.User = value;
			}
		}
	}
}

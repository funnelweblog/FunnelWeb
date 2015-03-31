using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using FunnelWeb.Web;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
	public class CustomResolver : IDependencyResolver
	{
		private readonly IContainer container;

		public CustomResolver(IContainer container)
		{
			this.container = container;
		}

		public object GetService(Type serviceType)
		{
			return container.Resolve(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			throw new NotSupportedException();
		}

		static bool initiated = false; 
		public static void Initiate()
		{
			//if (initiated) return;
			//initiated = true;
			var dependencyResolver = DependencyResolver.Current as IDisposable;
			if (dependencyResolver != null)
			{
				dependencyResolver.Dispose();
			}
			var container = MvcApplication.BuildContainer();
			DependencyResolver.SetResolver(new CustomResolver(container));
		}
	}
}
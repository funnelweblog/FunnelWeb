using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Web.Application.Mvc
{
    public class FunnelWebControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public FunnelWebControllerFactory(IContainer container)
        {
            _container = container;
        }
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var controller = base.GetControllerType(requestContext, controllerName);
            if (controller == null)
            {
                object x;
                if (_container.TryResolveNamed(controllerName, typeof(IController), out x))
                    controller = x.GetType();
            }

            return controller;
        }
    }
}
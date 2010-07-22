using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Web;
using NHibernate;

namespace FunnelWeb.Web.Application.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionalAttribute : ActionFilterAttribute
    {
        public TransactionalAttribute()
        {
        }

        protected ILifetimeScope Container
        {
            get { return ((IContainerProviderAccessor)HttpContext.Current.ApplicationInstance).ContainerProvider.RequestLifetime; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Container.Resolve<ISession>();
            session.BeginTransaction(IsolationLevel.Serializable);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                var session = Container.Resolve<ISession>();
                session.Flush();
                session.Transaction.Commit();
            }
            else
            {
                var session = Container.Resolve<ISession>();
                session.Clear();
                session.Transaction.Rollback();
            }
        }
    }
}
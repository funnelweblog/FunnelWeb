using System;
using System.Data;
using System.Web.Mvc;
using NHibernate;

namespace FunnelWeb.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FunnelWebRequestAttribute : ActionFilterAttribute
    {
        private readonly Func<ISession> sessionFinder;

        public FunnelWebRequestAttribute()
        {
            sessionFinder = () => DependencyResolver.Current.GetService<ISession>();
        }

        public FunnelWebRequestAttribute(Func<ISession> sessionFinder)
        {
            this.sessionFinder = sessionFinder;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = sessionFinder();
            session.BeginTransaction(IsolationLevel.Serializable);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                var session = sessionFinder();
                session.Flush();
                session.Transaction.Commit();
            }
            else
            {
                var session = sessionFinder();
                session.Clear();
                session.Transaction.Rollback();
            }
        }
    }
}
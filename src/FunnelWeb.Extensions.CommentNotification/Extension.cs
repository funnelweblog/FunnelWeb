using Autofac;
using FunnelWeb.Eventing;

namespace FunnelWeb.Extensions.CommentNotification
{
    [FunnelWebExtension(FullName = "Comment Notifications via Email", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class Extension : IFunnelWebExtension
    {
        public void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<CommentPostedListener>().As<IEventListener>().InstancePerLifetimeScope();
        }

        public string FullName { get; set; }
        public string SupportLink { get; set; }
        public string Publisher { get; set; }
    }
}
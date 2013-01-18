using Autofac;
using FunnelWeb.Eventing;

namespace FunnelWeb.Extensions.CommentNotification
{
    [FunnelWebExtension(FullName = "Notify admin via email on new comments", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class Extension : IFunnelWebExtension
    {
        public void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<NotifyAdminOfNewComment>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
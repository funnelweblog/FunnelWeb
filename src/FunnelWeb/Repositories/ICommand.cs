using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface ICommand
    {
        void Execute(ISession session);
    }
}
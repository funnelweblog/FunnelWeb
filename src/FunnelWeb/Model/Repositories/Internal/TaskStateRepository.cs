using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class TaskStateRepository : ITaskStateRepository
    {
        private readonly ISession session;

        public TaskStateRepository(ISession session)
        {
            this.session = session;
        }

        public TaskState Get(int id)
        {
            return session.Load<TaskState>(id);
        }

        public IQueryable<TaskState> GetAll()
        {
            return session.Query<TaskState>();
        }

        public void Save(TaskState state)
        {
            session.SaveOrUpdate(state);
        }
    }
}
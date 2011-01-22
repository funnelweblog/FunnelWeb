using System.Linq;

namespace FunnelWeb.Model.Repositories
{
    public interface ITaskStateRepository
    {
        TaskState Get(int id);
        IQueryable<TaskState> GetAll();
        void Save(TaskState state);
    }
}

using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Admin
{
    public class TaskModel
    {
        public TaskState State { get; set; }

        public TaskModel(TaskState state)
        {
            State = state;
        }
    }
}
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
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
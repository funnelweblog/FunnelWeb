using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class TasksModel
    {
        public List<TaskState> Tasks { get; set; }

        public TasksModel(List<TaskState> tasks)
        {
            Tasks = tasks;
        }
    }
}
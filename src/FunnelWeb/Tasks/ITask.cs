using System.Collections.Generic;

namespace FunnelWeb.Tasks
{
    public interface ITask
    {
        IEnumerable<TaskStep> Execute(Dictionary<string, object> properties);
    }
}
namespace FunnelWeb.Tasks
{
    public interface ITaskExecutor<out TTask> where TTask : ITask
    {
        int Execute(object arguments);
    }
}
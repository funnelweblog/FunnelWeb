namespace FunnelWeb.Tasks
{
    public interface ITaskExecutor<out TTask> where TTask : ITask
    {
        void Execute(object arguments);
    }
}
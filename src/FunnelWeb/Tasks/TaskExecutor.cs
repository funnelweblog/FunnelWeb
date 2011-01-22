using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Autofac;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using NHibernate;

namespace FunnelWeb.Tasks
{
    public class TaskExecutor<TTask> : ITaskExecutor<TTask> where TTask : ITask
    {
        private readonly ILifetimeScope rootScope;

        public TaskExecutor(ILifetimeScope rootScope)
        {
            this.rootScope = rootScope;
        }

        public int Execute(object arguments)
        {
            var properties = GetProperties(arguments);
            var taskId = CreateNewTaskState(properties);

            properties["taskId"] = taskId;
            ThreadPool.QueueUserWorkItem(RunOnBackgroundThread, properties);

            return taskId;
        }

        private void RunOnBackgroundThread(object arguments)
        {
            var properties = (Dictionary<string, object>)arguments;
            var taskId = (int)properties["taskId"];

            using (var scope = rootScope.BeginLifetimeScope())
            {
                var task = scope.Resolve<TTask>();

                try
                {
                    foreach (var step in task.Execute(properties))
                    {
                        var localStep = step;

                        UpdateTaskState(
                            taskId,
                            state =>
                            {
                                state.Append(localStep.LogMessage);
                                if (localStep.ProgressEstimate.HasValue)
                                {
                                    state.ProgressEstimate = localStep.ProgressEstimate.Value;
                                }
                            });
                    }

                    UpdateTaskState(
                        taskId,
                        state =>
                        {
                            state.Append("Completed");
                            state.Status = TaskStatus.Success;
                            state.ProgressEstimate = 100;
                        });
                }
                catch (Exception ex)
                {
                    UpdateTaskState(
                        taskId,
                        state =>
                        {
                            state.Append("Failed. {0}", ex);
                            state.Status = TaskStatus.Failed;
                            state.ProgressEstimate = 100;
                        });
                }
            }
        }

        private int CreateNewTaskState(Dictionary<string, object> properties)
        {
            var taskState = new TaskState();
            taskState.TaskName = typeof(TTask).Name;
            taskState.Arguments = string.Join(Environment.NewLine, properties.Select(x => x.Key + " = " + x.Value));
            taskState.Updated = taskState.Started = DateTime.UtcNow;
            taskState.OutputLog = string.Empty;
            taskState.ProgressEstimate = 0;
            taskState.Status = TaskStatus.Running;

            WithinTransaction(
                scope =>
                {
                    var repository = scope.Resolve<ITaskStateRepository>();
                    repository.Save(taskState);
                });

            return taskState.Id;
        }

        private void WithinTransaction(Action<ILifetimeScope> doWork)
        {
            using (var scope = rootScope.BeginLifetimeScope())
            {
                var session = scope.Resolve<ISession>();

                using (var tx = session.BeginTransaction())
                {
                    doWork(scope);

                    session.Flush();
                    tx.Commit();
                }
            }
        }

        private void UpdateTaskState(int taskId, Action<TaskState> updater)
        {
            WithinTransaction(
                scope =>
                {
                    var repository = scope.Resolve<ITaskStateRepository>();
                    var state = repository.Get(taskId);

                    updater(state);

                    repository.Save(state);
                });
        }

        private static Dictionary<string, object> GetProperties(object o)
        {
            if (o == null) return new Dictionary<string, object>();
            var results = new Dictionary<string, object>();
            var props = TypeDescriptor.GetProperties(o);
            foreach (PropertyDescriptor prop in props)
            {
                var val = prop.GetValue(o);
                if (val == null) continue;
                results.Add(prop.Name, val);
            }
            return results;
        }
    }
}
using System;
using Autofac;

namespace FunnelWeb.Tasks
{
    public class TasksModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof (TasksModule).Assembly)
                .AssignableTo<ITask>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof (TaskExecutor<>)).As(typeof (ITaskExecutor<>))
                .SingleInstance();
        }
    }
}
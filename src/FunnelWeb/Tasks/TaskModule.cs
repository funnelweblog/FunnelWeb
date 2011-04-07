using System;
using Autofac;

namespace FunnelWeb.Tasks
{
    public class TasksModule : Module
    {
        private readonly Func<IContainer> rootContainer;

        public TasksModule(Func<IContainer> rootContainer)
        {
            this.rootContainer = rootContainer;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof (TasksModule).Assembly)
                .AssignableTo<ITask>();

            builder.RegisterGeneric(typeof (TaskExecutor<>)).As(typeof (ITaskExecutor<>))
                .WithParameter(
                    (p, c) => p.Name == "rootScope",
                    (p, c) => rootContainer());
        }
    }
}
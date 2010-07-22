using NHibernate.Validator.Engine;
using Autofac;

namespace FunnelWeb.Web.Application.Validation
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => new NHibernateValidator(new ValidatorEngine())).As<IEntityValidator>();
        }
    }
}

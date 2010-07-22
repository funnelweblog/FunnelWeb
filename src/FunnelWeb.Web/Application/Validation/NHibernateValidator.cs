using NHibernate.Validator.Engine;

namespace FunnelWeb.Web.Application.Validation
{
    public class NHibernateValidator : IEntityValidator
    {
        private readonly ValidatorEngine _engine;

        public NHibernateValidator(ValidatorEngine engine)
        {
            _engine = engine;
        }

        public ValidationResult Validate(object source)
        {
            var results = _engine.Validate(source);
            return new ValidationResult(results);
        }
    }
}
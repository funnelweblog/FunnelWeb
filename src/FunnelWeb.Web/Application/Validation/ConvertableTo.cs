using System;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;
using System.ComponentModel;

namespace FunnelWeb.Web.Application.Validation
{
    [Serializable]
    [ValidatorClass(typeof(ConvertableValidator)), AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConvertableToAttribute : Attribute, IRuleArgs
    {
        public ConvertableToAttribute(Type type)
        {
            Type = type;
        }

        public string Message { get; set; }
        public Type Type { get; set; }
    }

    public class ConvertableValidator : IInitializableValidator<ConvertableToAttribute>, IValidator, IPropertyConstraint
    {
        private Type _type;

        public void Initialize(ConvertableToAttribute parameters)
        {
            _type = parameters.Type;
        }

        public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
        {
            return false;
        }

        public void Apply(Property property)
        {
            
        }
    }
}

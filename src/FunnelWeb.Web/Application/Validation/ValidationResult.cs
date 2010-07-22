using System.Collections;
using System.Collections.Generic;
using NHibernate.Validator.Engine;

namespace FunnelWeb.Web.Application.Validation
{
    public class ValidationResult : IEnumerable<InvalidValue>
    {
        private readonly List<InvalidValue> _validationErrors = new List<InvalidValue>();

        public ValidationResult(params InvalidValue[] validationErrors)
        {
            _validationErrors.AddRange(validationErrors);
        }

        public bool IsValid
        {
            get { return _validationErrors.Count == 0; }
        }

        public void Add(InvalidValue value)
        {
            _validationErrors.Add(value);
        }

        public IEnumerator<InvalidValue> GetEnumerator()
        {
            return _validationErrors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
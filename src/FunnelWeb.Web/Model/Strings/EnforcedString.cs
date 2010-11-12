using System;

namespace FunnelWeb.Web.Model.Strings
{
    public abstract class EnforcedString : IEquatable<EnforcedString>, IEquatable<string>
    {
        private readonly string value;

        protected EnforcedString(string value)
        {
            this.value = Correct(value);
        }

        protected abstract string Correct(string value);

        public bool Equals(EnforcedString other)
        {
            if (other == null) return false;
            return Equals(other.value);
        }

        public bool Equals(string other)
        {
            return string.Equals(value, other, StringComparison.CurrentCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is string) return Equals((string)obj);
            if (obj is EnforcedString) return Equals((EnforcedString)obj);
            return Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return value;
        }
    }
}
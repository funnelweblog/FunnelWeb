using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FunnelWeb.Model.Strings
{
    /// <summary>
    /// PageName class is an all-lowercase string which does not have any special characters (except - and /)
    /// </summary>
    [Serializable]
    public sealed class PageName : EnforcedString, IEquatable<PageName>
    {
        private PageName(string value)
            : base(value)
        {
        }

        protected override string Correct(string value)
        {
            value = value ?? string.Empty;
            value = value.ToLower();
            value = Regex.Replace(value, "\\s", "-");
            value = new string(value.Select(x => (char.IsLetterOrDigit(x) || x == '-' || x == '/') ? x : '-').ToArray());
            value = Regex.Replace(value, "-+", "-");
            value = value.Trim('-', '/');
            return value;
        }

        public static implicit operator PageName(string value)
        {
            return new PageName((value ?? string.Empty).Trim());
        }

        public static implicit operator string(PageName name)
        {
            return name.ToString();
        }

        public static bool operator ==(PageName left, PageName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PageName left, PageName right)
        {
            return !Equals(left, right);
        }

        public bool Equals(PageName other)
        {
            return base.Equals((EnforcedString)other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PageName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

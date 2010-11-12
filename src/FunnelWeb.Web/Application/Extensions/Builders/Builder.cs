using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Xml;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public abstract class Builder
    {
        private readonly HtmlHelper _html;
        private readonly string _name;
        private readonly List<string> _classes = new List<string>();

        protected Builder(HtmlHelper html, string name)
        {
            _html = html;
            _name = name;
        }

        public string Name { get { return _name; } }
        public HtmlHelper Html { get { return _html; } }

        protected string GetLastValueOrDefault(string name, string defaultValue)
        {
            return Html.ViewContext.HttpContext.Request.Form.AllKeys.Contains(name) 
                ? Html.ViewContext.HttpContext.Request.Form[name] 
                : defaultValue;
        }

        public void AddClass(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _classes.Add(name);
            }
        }

        protected string AllClasses
        {
            get { return string.Join(" ", _classes.Distinct().ToArray()); }
        }

        public string Render()
        {
            return ToString();
        }

        protected abstract void Render(XmlTextWriter writer);

        public override string ToString()
        {
            var strings = new StringWriter();
            var builder = new XmlTextWriter(strings);
            Render(builder);
            return strings.ToString();
        }
    }
}
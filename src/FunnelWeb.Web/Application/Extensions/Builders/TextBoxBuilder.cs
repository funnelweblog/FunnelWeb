using System.Web.UI;
using System.Xml;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public class TextBoxBuilder : Builder, IFieldBuilder<TextBoxBuilder>
    {
        private string _value;
        private int? _max;
        private string _alt;

        public TextBoxBuilder(HtmlHelper html, string name) : base(html, name)
        {
        }

        /// <summary>
        /// Good for 5 characters or less.
        /// </summary>
        public TextBoxBuilder XSmall()
        {
            AddClass("xsmall");
            return this;
        }

        /// <summary>
        /// Good for 15 characters or less.
        /// </summary>
        public TextBoxBuilder Small()
        {
            AddClass("small");
            return this;
        }

        /// <summary>
        /// Good for 30 characters or less.
        /// </summary>
        public TextBoxBuilder Medium()
        {
            AddClass("medium");
            return this;
        }

        /// <summary>
        /// Good for 100 characters or less.
        /// </summary>
        public TextBoxBuilder Large()
        {
            AddClass("large");
            return this;
        }

        public TextBoxBuilder Default(object value)
        {
            _value = (value ?? string.Empty).ToString();
            return this;
        }

        public TextBoxBuilder Max(int max)
        {
            _max = max;
            AddClass("restricted-length");
            return this;
        }
             
        public TextBoxBuilder IsRequired()
        {
            AddClass("required");
            return this;
        }

        public TextBoxBuilder Alt(string altText)
        {
            _alt = altText;
            return this;
        }

        protected override void Render(XmlTextWriter writer)
        {
            writer.WriteStartElement("input");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("id", Name);
            writer.WriteAttributeString("class", AllClasses);
            writer.WriteAttributeString("value", GetLastValueOrDefault(Name, _value));
            if (_max.HasValue)
            {
                writer.WriteAttributeString("maxlength", _max.ToString());
            }
            if (!string.IsNullOrEmpty(_alt))
            {
                writer.WriteAttributeString("alt", _alt);
            }
            writer.WriteEndElement();
        }
    }
}
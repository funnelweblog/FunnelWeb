using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using System;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public class TextAreaBuilder : Builder, IFieldBuilder<TextAreaBuilder>
    {
        private string _value;

        public TextAreaBuilder(HtmlHelper html, string name)
            : base(html, name)
        {
        }

        /// <summary>
        /// Good for 15 characters or less.
        /// </summary>
        public TextAreaBuilder Small()
        {
            AddClass("small");
            return this;
        }

        /// <summary>
        /// Good for 100 characters or less.
        /// </summary>
        public TextAreaBuilder Medium()
        {
            AddClass("large");
            return this;
        }

        public TextAreaBuilder Default(object value)
        {
            _value = (value ?? string.Empty).ToString();
            return this;
        }

        public TextAreaBuilder IsRequired()
        {
            AddClass("required");
            return this;
        }

        protected override void Render(XmlTextWriter writer)
        {
            if (_value.Length == 0) 
                _value = Environment.NewLine;
            
            writer.WriteStartElement("textarea");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("id", Name);
            writer.WriteAttributeString("class", AllClasses);
            writer.WriteString(_value);
            writer.WriteEndElement();
        }
    }
}

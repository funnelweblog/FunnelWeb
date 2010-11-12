using System.Web.UI;
using System.Xml;
using System;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public class TextEditorBuilder : Builder, IFieldBuilder<TextEditorBuilder>
    {
        private string _value;

        public TextEditorBuilder(HtmlHelper html, string name) 
            : base(html, name)
        {
            AddClass("wmd-panel");
        }

        public TextEditorBuilder Default(object value)
        {
            _value = (value ?? string.Empty).ToString();
            return this;
        }

        public TextEditorBuilder IsRequired()
        {
            AddClass("required");
            return this;
        }

        protected override void Render(XmlTextWriter writer)
        {
            writer.WriteStartElement("div");
            writer.WriteAttributeString("id", "wmd-button-bar");
            writer.WriteAttributeString("class", "wmd-panel");
            writer.WriteString(Environment.NewLine);
            writer.WriteEndElement();

            writer.WriteStartElement("textarea");
            writer.WriteAttributeString("id", "wmd-input");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("cols", "80");
            writer.WriteAttributeString("rows", "18");
            writer.WriteAttributeString("class", AllClasses);
            writer.WriteString(_value);
            writer.WriteString(Environment.NewLine);
            writer.WriteEndElement();
        }
    }
}

using System;
using System.Web.UI;
using System.Xml;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public class DatePickerBuilder : Builder, IFieldBuilder<DatePickerBuilder>
    {
        private string _value;

        public DatePickerBuilder(HtmlHelper html, string name)
            : base(html, name)
        {
            AddClass("dateISO");
        }

        public DatePickerBuilder Default(object value)
        {
            if (value is DateTime)
            {
                value = ((DateTime)value).ToString("yyyy-MM-dd");
            }
            _value = (value ?? string.Empty).ToString();
            return this;
        }

        public DatePickerBuilder IsRequired()
        {
            AddClass("required");
            return this;
        }

        protected override void Render(XmlTextWriter writer)
        {
            writer.WriteStartElement("input");
            writer.WriteAttributeString("name", Name);
            writer.WriteAttributeString("id", Name);
            writer.WriteAttributeString("class", AllClasses);
            writer.WriteAttributeString("value", _value);
            writer.WriteEndElement();
        }
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace FunnelWeb.Web.Application.Extensions.Builders
{
    public class MessageListBuilder : Builder
    {
        private readonly List<string> _messages = new List<string>();

        public MessageListBuilder(HtmlHelper html) : base(html, string.Empty)
        {
        }

        public MessageListBuilder Append(string message)
        {
            message = (message ?? string.Empty).Trim();
            if (message.Length == 0)
            {
                return this;
            }
            
            if (!message.EndsWith("."))
            {
                message = message + ".";
            }

            if (!_messages.Contains(message))
            {
                _messages.Add(message);
            }
            return this;
        }

        protected override void Render(XmlTextWriter writer)
        {
            writer.WriteStartElement("div");
            writer.WriteAttributeString("class", AllClasses);
            writer.WriteStartElement("ul");

            foreach (var message in _messages)
            {
                writer.WriteStartElement("li");
                writer.WriteString(Html.Encode(message));
                writer.WriteEndElement();   
            }
            
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}

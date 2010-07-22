using System.Collections.Generic;

namespace FunnelWeb.Web.Application.Extensions
{
    public class FlashData
    {
        private readonly List<string> _messages = new List<string>();

        public void Add(string message, params object[] formatArguments)
        {
            var text = string.Format(message, formatArguments);
            _messages.Add(text);
        }

        public IEnumerable<string> Messages
        {
            get { return _messages; }
        }
    }
}
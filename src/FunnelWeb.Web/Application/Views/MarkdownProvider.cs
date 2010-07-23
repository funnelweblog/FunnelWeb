using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FunnelWeb.Web.Application.Views
{
    public class MarkdownProvider : IMarkdownProvider
    {
        public MarkdownProvider(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        string _baseUrl;

        public string Render(string text, bool sanitize = false)
        {
            return new MarkdownRenderer(sanitize, _baseUrl).Render(text);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FunnelWeb.Web.Application.Views
{
    public interface IMarkdownProvider
    {
        string Render(string text, bool sanitize = false);
    }
}
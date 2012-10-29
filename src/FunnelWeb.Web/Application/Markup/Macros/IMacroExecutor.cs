using System.Text;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Markup.Macros
{
    public interface IMacroExecutor
    {
        StringBuilder ExecuteMacro(string templateContent, HtmlHelper html);
    }
}
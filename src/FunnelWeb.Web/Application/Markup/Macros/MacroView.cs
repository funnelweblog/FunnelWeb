using System.Text;
using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Markup.Macros
{
    public class MacroView
    {
        private StringBuilder output;

        public HtmlHelper Html { get; set; }

        public void Initialize(StringBuilder writer)
        {
            output = writer;
        }

        protected virtual void WriteLiteral(object o)
        {
            output.Append(o);
        }

        protected virtual void Write(object o)
        {
            output.Append(o);
        }

        public virtual void Execute()
        {

        }
    }
}

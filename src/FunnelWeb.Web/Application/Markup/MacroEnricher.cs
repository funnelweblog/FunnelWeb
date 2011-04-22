using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Web.Application.Markup.Macros;

namespace FunnelWeb.Web.Application.Markup
{
    public class MacroEnricher : IContentEnricher
    {
        private readonly HttpContextBase context;
        private static readonly Regex macroRecognizerRegex = new Regex(@"\[Macro:\s*([A-z0-9]+)\s*({.*?})?\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public MacroEnricher(HttpContextBase context)
        {
            this.context = context;
        }

        public string Enrich(string content, bool isContentTrusted, HtmlHelper html)
        {
            if (!isContentTrusted)
            {
                // Only evaluate macros in trusted content (e.g., posts by the site admin)
                return content;
            }

            MatchEvaluator matchEval = delegate(Match m)
            {
                string finalMacroContent = string.Empty;
                try
                {
                    var macroName = m.Result("$1");
                    var macroArguments = m.Result("$2");
                    if (string.IsNullOrWhiteSpace(macroArguments))
                    {
                        macroArguments = "{x = \"42\"}";
                    }

                    var macroFilePath = context.Server.MapPath("~/Macros/" + macroName + ".cshtml");
                    finalMacroContent = "@{ var Args = new " + macroArguments + "; }" + Environment.NewLine + File.ReadAllText(macroFilePath);

                    return new RazorMacroExecutor().ExecuteMacro(finalMacroContent, html).ToString();
                }
                catch (Exception ex)
                {
                    return "<div class='macro-error'><pre>" + ex + "</pre><p>Macro content:</p><pre>" + finalMacroContent + "</pre></div>";
                }
            };

            return macroRecognizerRegex.Replace(content, matchEval);
        }
    }
}
namespace FunnelWeb.Web.Application.Markup
{
    public class MacroEvaluator : IContentEnricher
    {
        public string Enrich(string content, bool isContentTrusted)
        {
            if (!isContentTrusted)
            {
                // Only evaluate macros in trusted content (e.g., posts by the site admin)
                return content;
            }

            return content.Replace("[Google]", "<iframe src='http://www.google.com'></iframe>");
        }
    }
}
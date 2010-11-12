namespace FunnelWeb.Web.Application.Views
{
    public class MarkdownProvider : IMarkdownProvider
    {
        private readonly string baseUrl;

        public MarkdownProvider(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public string Render(string text, bool sanitize = false)
        {
            return new MarkdownRenderer(sanitize, baseUrl).Render(text);
        }
    }
}
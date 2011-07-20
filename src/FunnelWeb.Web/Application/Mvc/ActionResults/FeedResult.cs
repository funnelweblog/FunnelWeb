using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace FunnelWeb.Web.Application.Mvc.ActionResults
{
    public class FeedResult : ActionResult
    {
        private readonly SyndicationFeedFormatter feed;
        private readonly DateTime lastModified;

        public FeedResult(SyndicationFeedFormatter feed, DateTime lastModified)
        {
            this.feed = feed;
            this.lastModified = lastModified;
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }

        public SyndicationFeedFormatter Feed
        {
            get { return feed; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/rss+xml";
            context.HttpContext.Response.Cache.SetLastModified(lastModified);

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (feed == null) 
                return;

            using (var xmlWriter = new XmlTextWriter(response.Output))
            {
                xmlWriter.Formatting = Formatting.Indented;
                feed.WriteTo(xmlWriter);
            }
        }
    }

}
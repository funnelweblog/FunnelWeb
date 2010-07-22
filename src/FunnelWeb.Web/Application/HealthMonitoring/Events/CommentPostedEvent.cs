using System.Web.Management;

namespace FunnelWeb.Web.Application.HealthMonitoring.Events
{
    public class CommentPostedEvent : WebBaseEvent
    {
        private readonly string _comment;

        public CommentPostedEvent(string entryName, bool isSpam, string comment, object source)
            : base(string.Format("[{0}] /{1}", isSpam ? "Spam" : "Comment", entryName), source, WebEventCodes.WebExtendedBase + 5010)
        {
            _comment = comment;
        }

        public string Comment
        {
            get { return _comment; }
        }
    }
}
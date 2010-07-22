using System.Web.Management;

namespace FunnelWeb.Web.Application.HealthMonitoring.Events
{
    public class PingbackPostedEvent : WebBaseEvent
    {
        private readonly string _targetUri;

        public PingbackPostedEvent(string entryName, bool isSpam, string targetUri, object source)
            : base(string.Format("[{0}] /{1}", isSpam ? "Pingback Spam" : "Pingback", entryName), source, WebEventCodes.WebExtendedBase + 5010)
        {
            _targetUri = targetUri;
        }

        public string TargetUri
        {
            get { return _targetUri; }
        }
    }
}
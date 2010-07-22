using System.Web.Management;

namespace FunnelWeb.Web.Application.HealthMonitoring.Events
{
    public class EntryNotFoundEvent : WebBaseEvent
    {
        public EntryNotFoundEvent(string entryName, object source)
            : base(string.Format("[404] {0}", entryName), source, WebEventCodes.WebExtendedBase + 5001)
        {
            
        }
    }
}

namespace FunnelWeb.Web.Application
{
    public interface IDatabaseUpgradeDetector
    {
        bool UpdateNeeded();
    }
}

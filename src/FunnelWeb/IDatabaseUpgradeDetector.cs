namespace FunnelWeb
{
    public interface IDatabaseUpgradeDetector
    {
        bool UpdateNeeded();
        void Reset();
    }
}

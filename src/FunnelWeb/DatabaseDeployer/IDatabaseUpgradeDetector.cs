namespace FunnelWeb.DatabaseDeployer
{
	public interface IDatabaseUpgradeDetector
	{
		bool UpdateNeeded();
		void Reset();
	}
}

namespace FunnelWeb.DatabaseDeployer
{
	public interface IDatabaseConnectionDetector
	{
		bool CanConnect(out string error);
	}
}
namespace FunnelWeb.DatabaseDeployer
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; set; }
    }
}
namespace FunnelWeb.DatabaseDeployer
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; set; }
        string Schema { get; set; }
    }
}
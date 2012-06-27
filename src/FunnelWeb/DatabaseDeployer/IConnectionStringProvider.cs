namespace FunnelWeb.DatabaseDeployer
{
    public interface IConnectionStringProvider
    {
        string DatabaseProvider { get; set; }
        string ConnectionString { get; set; }
        string Schema { get; set; }
        string ReadOnlyReason { get; }
    }
}
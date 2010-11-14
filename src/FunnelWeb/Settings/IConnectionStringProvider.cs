namespace FunnelWeb.Settings
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; set; }
    }
}
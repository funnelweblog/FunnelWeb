using FunnelWeb.Providers.Database;

namespace FunnelWeb.DatabaseDeployer
{
	public class DatabaseConnectionDetector : IDatabaseConnectionDetector
	{
		private readonly IConnectionStringSettings connectionStringSettings;
		private readonly IDatabaseProvider databaseProvider;

		public DatabaseConnectionDetector(
			IConnectionStringSettings connectionStringSettings,
			IDatabaseProvider databaseProvider)
		{
			this.connectionStringSettings = connectionStringSettings;
			this.databaseProvider = databaseProvider;
		}

		public bool CanConnect(out string error)
		{
			var connectionString = connectionStringSettings.ConnectionString;
			return databaseProvider.TryConnect(connectionString, out error);
		}
	}
}
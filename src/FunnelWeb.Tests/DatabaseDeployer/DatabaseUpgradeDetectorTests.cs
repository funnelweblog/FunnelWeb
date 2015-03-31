using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DbUp.Engine;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Providers.Database;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.DatabaseDeployer
{
	[TestFixture]
	public class DatabaseUpgradeDetectorTests
	{
		private DatabaseUpgradeDetector detector;
		private IConnectionStringSettings connectionString;
		private IApplicationDatabase applicationDatabase;
		private readonly List<ScriptedExtension> extensions = new List<ScriptedExtension>();
		private IDatabaseProvider databaseProvider;
		private IDatabaseConnectionDetector databaseConnectionDetector;

		[SetUp]
		public void SetUp()
		{
			connectionString = Substitute.For<IConnectionStringSettings>();
			connectionString.Schema = "dbo";
			connectionString.DatabaseProvider = "sql";
			applicationDatabase = Substitute.For<IApplicationDatabase>();
			databaseProvider = Substitute.For<IDatabaseProvider>();
			databaseConnectionDetector = Substitute.For<IDatabaseConnectionDetector>();
			detector = new DatabaseUpgradeDetector(connectionString, extensions, applicationDatabase, databaseProvider, databaseConnectionDetector);
		}

		[Test]
		public void UpdateNeededIfDatabaseIsOffline()
		{
			DatabaseIsOnline(false);

			var needed = detector.UpdateNeeded();
			Assert.IsTrue(needed);
		}

		[Test]
		public void UpdateNeededIfMainDatabaseIsOld()
		{
			DatabaseIsOnline(true);
			CurrentSchemaVersionIs(10);
			RequiredApplicationVersionIs(20);

			var needed = detector.UpdateNeeded();
			Assert.IsTrue(needed);
		}

		[Test]
		public void UpdateNotNeededIfUpToDate()
		{
			DatabaseIsOnline(true);
			CurrentSchemaVersionIs(10);
			RequiredApplicationVersionIs(10);

			var needed = detector.UpdateNeeded();
			Assert.IsFalse(needed);
		}

		[Test]
		public void UpdateNeededIfExtensionsOld()
		{
			extensions.Add(new ScriptedExtension("XYZ", null, Substitute.For<IScriptProvider>()));

			DatabaseIsOnline(true);
			CurrentSchemaVersionIs(10);
			RequiredApplicationVersionIs(10);
			CurrentExtensionVersionIs(1);
			RequiredExtensionVersionIs(4);

			var needed = detector.UpdateNeeded();
			Assert.IsTrue(needed);
		}

		[Test]
		public void UpdateNotNeededIfExtensionsUpToDate()
		{
			extensions.Add(new ScriptedExtension("XYZ", null, Substitute.For<IScriptProvider>()));

			DatabaseIsOnline(true);
			CurrentSchemaVersionIs(10);
			RequiredApplicationVersionIs(10);
			CurrentExtensionVersionIs(4);
			RequiredExtensionVersionIs(4);

			var needed = detector.UpdateNeeded();
			Assert.IsFalse(needed);
		}

		#region Helpers

		private void DatabaseIsOnline(bool isItReally)
		{
			string message;
			databaseProvider
					.TryConnect(Arg.Any<string>(), out message)
					.Returns(isItReally);
		}

		private void CurrentSchemaVersionIs(int version)
		{
			applicationDatabase
					.GetCoreExecutedScripts(Arg.Any<Func<IDbConnection>>())
					.Returns(Enumerable.Range(1, version).Select(x => "Script" + x + ".sql").ToArray());
		}

		private void RequiredApplicationVersionIs(int version)
		{
			applicationDatabase
					.GetCoreRequiredScripts(Arg.Any<Func<IDbConnection>>())
					.Returns(Enumerable.Range(1, version).Select(x => "Script" + x + ".sql").ToArray());
		}

		private void CurrentExtensionVersionIs(int version)
		{
			applicationDatabase
					.GetExtensionExecutedScripts(Arg.Any<Func<IDbConnection>>(), Arg.Any<ScriptedExtension>())
					.Returns(Enumerable.Range(1, version).Select(x => "Script" + x + ".sql").ToArray());
		}

		private void RequiredExtensionVersionIs(int version)
		{
			applicationDatabase
					.GetExtensionRequiredScripts(Arg.Any<Func<IDbConnection>>(), Arg.Any<ScriptedExtension>())
					.Returns(Enumerable.Range(1, version).Select(x => "Script" + x + ".sql").ToArray());
		}

		#endregion
	}
}

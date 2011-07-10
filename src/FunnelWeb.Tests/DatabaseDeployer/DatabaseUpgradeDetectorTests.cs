using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autofac.Features.Indexed;
using DbUp.Engine;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.DbProviders;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.DatabaseDeployer
{
    [TestFixture]
    public class DatabaseUpgradeDetectorTests
    {
        private DatabaseUpgradeDetector detector;
        private IConnectionStringProvider connectionString;
        private IApplicationDatabase applicationDatabase;
        private readonly List<ScriptedExtension> extensions = new List<ScriptedExtension>();
        private IIndex<string, IDatabaseProvider> databaseProviderLookup;
        private IDatabaseProvider databaseProvider;

        [SetUp]
        public void SetUp()
        {
            connectionString = Substitute.For<IConnectionStringProvider>();
            connectionString.Schema = "dbo";
            connectionString.DatabaseProvider = "sql";
            applicationDatabase = Substitute.For<IApplicationDatabase>();
            databaseProviderLookup = Substitute.For<IIndex<string, IDatabaseProvider>>();
            databaseProvider = Substitute.For<IDatabaseProvider>();
            databaseProviderLookup[Arg.Any<string>()].Returns(databaseProvider);
            detector = new DatabaseUpgradeDetector(connectionString, extensions, applicationDatabase, databaseProviderLookup);
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
                .GetCoreRequiredScripts()
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
                .GetExtensionRequiredScripts(Arg.Any<ScriptedExtension>())
                .Returns(Enumerable.Range(1, version).Select(x => "Script" + x + ".sql").ToArray());
        }

        #endregion
    }
}

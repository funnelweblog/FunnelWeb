using System.Collections.Generic;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
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
        private List<IScriptProvider> extensions = new List<IScriptProvider>();

        [SetUp]
        public void SetUp()
        {
            connectionString = Substitute.For<IConnectionStringProvider>();
            applicationDatabase = Substitute.For<IApplicationDatabase>();
            detector = new DatabaseUpgradeDetector(connectionString, extensions, applicationDatabase);
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
            extensions.Add(Substitute.For<IScriptProvider>());

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
            extensions.Add(Substitute.For<IScriptProvider>());

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
            applicationDatabase
                .TryConnect(Arg.Any<string>(), out message)
                .Returns(isItReally);
        }

        private void CurrentSchemaVersionIs(int version)
        {
            applicationDatabase
                .GetApplicationCurrentVersion(Arg.Any<string>())
                .Returns(version);
        }

        private void RequiredApplicationVersionIs(int version)
        {
            applicationDatabase
                .GetApplicationVersion()
                .Returns(version);
        }

        private void CurrentExtensionVersionIs(int version)
        {
            applicationDatabase
                .GetExtensionVersion(Arg.Any<IScriptProvider>())
                .Returns(version);
        }

        private void RequiredExtensionVersionIs(int version)
        {
            applicationDatabase
                .GetExtensionCurrentVersion(Arg.Any<string>(), Arg.Any<IScriptProvider>())
                .Returns(version);
        }

        #endregion
    }
}

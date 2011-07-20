using System;
using System.Web;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Settings;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Core.Repositories
{
    public class FileRepositoryTests
    {
        [Test]
        public void RelativePaths()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();
            server.MapPath(Arg.Is("~/Temp")).Returns("C:\\Temp");
            var settings = Substitute.For<ISettingsProvider>();
            settings.GetSettings<FunnelWebSettings>().Returns(new FunnelWebSettings { UploadPath = "~/Temp" });

            new FileRepository(settings, server);

            //Act

            //Assert
            server.Received().MapPath(Arg.Is("~/Temp"));
        }

        [Test]
        public void AbsolutePaths()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();
            var settings = Substitute.For<ISettingsProvider>();
            settings.GetSettings<FunnelWebSettings>().Returns(new FunnelWebSettings { UploadPath = "C:\\Temp" });
            
            new FileRepository(settings, server);

            //Act

            //Assert
            server.DidNotReceiveWithAnyArgs().MapPath(Arg.Any<string>());
        }
    }
}

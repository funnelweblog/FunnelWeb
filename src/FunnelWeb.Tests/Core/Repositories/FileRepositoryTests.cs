using System.Web;
using FunnelWeb.Model.Repositories.Internal;
using FunnelWeb.Settings;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Core.Model.Repositories
{
    public class FileRepositoryTests
    {
        [Test]
        public void RelativePaths()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();
            server.MapPath(Arg.Is<string>("~/Temp")).Returns("C:\\Temp");
            var settings = Substitute.For<ISettingsProvider>();
            settings.GetSettings<FunnelWebSettings>().Returns(new FunnelWebSettings { UploadPath = "~/Temp" });

            var fileRepo = new FileRepository(settings, server);

            //Act

            //Assert
            server.Received().MapPath(Arg.Is<string>("~/Temp"));
        }

        [Test]
        public void AbsolutePaths()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();
            var settings = Substitute.For<ISettingsProvider>();
            settings.GetSettings<FunnelWebSettings>().Returns(new FunnelWebSettings { UploadPath = "C:\\Temp" });
            
            var fileRepo = new FileRepository(settings, server);

            //Act

            //Assert
            server.DidNotReceiveWithAnyArgs().MapPath(Arg.Any<string>());
        }
    }
}

using System.Web;
using FunnelWeb.Web.Model.Repositories.Internal;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Model.Repositories
{
    public class FileRepositoryTests
    {
        [Test]
        public void FileRepositoryTests_Relative_Paths_Resolved_From_Server()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();
            server.MapPath(Arg.Is<string>("~/Temp")).Returns("C:\\Temp");

            var fileRepo = new FileRepository("~/Temp", server);

            //Act

            //Assert
            server.Received().MapPath(Arg.Is<string>("~/Temp"));
        }

        [Test]
        public void FileRepositoryTests_File_System_Paths_Ignore_Server()
        {
            //Arrange
            var server = Substitute.For<HttpServerUtilityBase>();

            var fileRepo = new FileRepository("C:\\Temp", server);

            //Act

            //Assert
            server.DidNotReceiveWithAnyArgs().MapPath(Arg.Any<string>());
        }
    }
}

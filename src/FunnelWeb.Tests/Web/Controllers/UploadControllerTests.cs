using System.Web;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using FunnelWeb.Web.Model.Repositories;
using FunnelWeb.Web.Controllers;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application;

namespace FunnelWeb.Tests.Web.Controllers
{
    public class UploadControllerTests
    {
        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            HttpContextBase httpContext = Substitute.For<HttpContextBase>();
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }

        [TestFixture]
        public class ActionResultTests
        {
            [Test]
            public void UploadControllerTests_Index_With_File_Returns_Index()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var fileRepo = Substitute.For<IFileRepository>();
                fileRepo.IsFile(Arg.Any<string>()).Returns(x => true);

                var controller = new UploadController(fileRepo, Substitute.For<IMimeTypeLookup>());
                //Act
                var result = (RedirectToRouteResult)controller.Index("test");

                //Assert
                fileRepo.Received().IsFile(Arg.Any<string>());
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void UploadControllerTests_Index_With_No_File_Returns_Index()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var fileRepo = Substitute.For<IFileRepository>();
                fileRepo.IsFile(Arg.Any<string>()).Returns(x => false);

                var controller = new UploadController(fileRepo, Substitute.For<IMimeTypeLookup>());
                //Act
                var result = (ViewResult)controller.Index("test");

                //Assert
                fileRepo.Received().IsFile(Arg.Any<string>());
                Assert.That(result.View, Is.EqualTo(null));
            }

            [Test]
            public void UploadControllerTests_Upload_Saves_And_Returns_Index()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var fileRepo = Substitute.For<IFileRepository>();
                var controller = new UploadController(fileRepo, Substitute.For<IMimeTypeLookup>());

                HttpPostedFileBase file = Substitute.For<HttpPostedFileBase>();
                var upload = new Upload(file);
                //Act
                var result = (RedirectToRouteResult)controller.Upload("path", upload);

                //Assert
                fileRepo.Received().MapPath(Arg.Is<string>("path"));
                file.Received().SaveAs(Arg.Any<string>());
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            }

            [Test]
            public void ActionResultTests_CreateDirectory_Via_Repo_And_Return_Index()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var fileRepo = Substitute.For<IFileRepository>();
                var controller = new UploadController(fileRepo, Substitute.For<IMimeTypeLookup>());

                //Act
                var result = (RedirectToRouteResult)controller.CreateDirectory("path", string.Empty);

                //Assert
                fileRepo.Received().CreateDirectory(Arg.Is<string>("path"), Arg.Any<string>());
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
            }

            [Test]
            public void ActionResultTests_Delete_Via_Repo_And_Return_Index()
            {
                //Arrange
                var controllerContext = CreateControllerContext();

                var fileRepo = Substitute.For<IFileRepository>();
                var controller = new UploadController(fileRepo, Substitute.For<IMimeTypeLookup>());

                //Act
                var result = (RedirectToRouteResult)controller.Delete("path", "file");

                //Assert
                fileRepo.Received().Delete(Arg.Is<string>("file"));
                Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
            }
        }
    }
}

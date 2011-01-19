using System.IO;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Controllers
{
    [TestFixture]
    public class UploadControllerTests
    {
        protected UploadController Controller { get; set; }
        protected IFileRepository FileRepository { get; set; }
        protected IMimeTypeLookup MimeTypeLookup { get; set; }

        [SetUp]
        public void SetUp()
        {
            Controller = new UploadController();
            Controller.FileRepository = FileRepository = Substitute.For<IFileRepository>();
            Controller.MimeHelper = MimeTypeLookup = Substitute.For<IMimeTypeLookup>();
            Controller.ControllerContext = CreateControllerContext();
        }

        [Test]
        public void IndexForExistingFile()
        {
            FileRepository.IsFile(Arg.Any<string>()).Returns(true);

            var result = (RedirectToRouteResult)Controller.Index("test");

            FileRepository.Received().IsFile(Arg.Any<string>());
            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void IndexForMissingFile()
        {
            FileRepository.IsFile(Arg.Any<string>()).Returns(false);

            var result = (ViewResult)Controller.Index("test");

            FileRepository.Received().IsFile(Arg.Any<string>());
            Assert.That(result.View, Is.EqualTo(null));
        }

        [Test]
        public void Upload()
        {
            var stream = new MemoryStream();

            var file = Substitute.For<HttpPostedFileBase>();
            file.InputStream.Returns(stream);

            var upload = new FileUpload(file);

            FileRepository.MapPath(Arg.Any<string>()).Returns("path");
            
            var result = (RedirectToRouteResult)Controller.Upload("path", false, upload);

            FileRepository.Received().MapPath(Arg.Is("path"));
            FileRepository.Received().Save(Arg.Is<Stream>(stream), Arg.Is("path"), Arg.Is(false));

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void UploadAndUnzip()
        {
            var stream = new MemoryStream();

            var file = Substitute.For<HttpPostedFileBase>();
            file.InputStream.Returns(stream);

            var upload = new FileUpload(file);

            FileRepository.MapPath(Arg.Any<string>()).Returns("path");

            var result = (RedirectToRouteResult)Controller.Upload("path", true, upload);

            FileRepository.Received().MapPath(Arg.Is("path"));
            FileRepository.Received().Save(Arg.Is<Stream>(stream), Arg.Is("path"), Arg.Is(true));

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void CreateDirectory()
        {
            var result = (RedirectToRouteResult)Controller.CreateDirectory("path", string.Empty);

            FileRepository.Received().CreateDirectory(Arg.Is<string>("path"), Arg.Any<string>());
            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
        }

        [Test]
        public void Delete()
        {
            var result = (RedirectToRouteResult)Controller.Delete("path", "file");

            FileRepository.Received().Delete(Arg.Is<string>("file"));
            Assert.That((string)result.RouteValues["Action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
        }

        [Test]
        public void RenderExistingFile()
        {
            FileRepository.IsFile(Arg.Is<string>("file")).Returns(true);
            FileRepository.MapPath(Arg.Is<string>("file")).Returns("file");

            MimeTypeLookup.GetMimeType(Arg.Any<string>()).Returns("mime-type");

            var result = (FilePathResult)Controller.Render("file");

            FileRepository.Received().IsFile(Arg.Is<string>("file"));
            MimeTypeLookup.Received().GetMimeType(Arg.Any<string>());

            Assert.That(result.FileName, Is.EqualTo("file"));
            Assert.That(result.ContentType, Is.EqualTo("mime-type"));
        }

        [Test]
        public void RenderMissingFile()
        {
            FileRepository.IsFile(Arg.Is<string>("file")).Returns(false);

            var result = (RedirectResult)Controller.Render("file");

            FileRepository.Received().IsFile(Arg.Is<string>("file"));
            FileRepository.DidNotReceive().MapPath(Arg.Any<string>());
            MimeTypeLookup.DidNotReceive().GetMimeType(Arg.Any<string>());

            Assert.That(result.Url, Is.EqualTo("/"));
        }

        private static ControllerContext CreateControllerContext()
        {
            var controllerContext = new ControllerContext();
            var httpContext = Substitute.For<HttpContextBase>();
            controllerContext.HttpContext = httpContext;
            return controllerContext;
        }
    }
}

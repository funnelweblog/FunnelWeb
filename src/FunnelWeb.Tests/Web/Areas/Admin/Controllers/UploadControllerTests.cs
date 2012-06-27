using System.IO;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Tests.Web.Controllers;
using FunnelWeb.Utilities;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Areas.Admin.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace FunnelWeb.Tests.Web.Areas.Admin.Controllers
{
    [TestFixture]
    public class UploadControllerTests : ControllerTests
    {
        protected UploadController Controller { get; set; }
        protected IFileRepository FileRepository { get; set; }
        protected IMimeTypeLookup MimeTypeLookup { get; set; }

        [SetUp]
        public void SetUp()
        {
            Controller = new UploadController
                             {
                                 FileRepository = FileRepository = Substitute.For<IFileRepository>(),
                                 MimeHelper = MimeTypeLookup = Substitute.For<IMimeTypeLookup>(),
                                 ControllerContext = ControllerContext
                             };
        }

        [Test]
        public void IndexForExistingFile()
        {
            FileRepository.IsFile(Arg.Any<string>()).Returns(true);

            var result = (RedirectToRouteResult)Controller.Index("test");

            FileRepository.Received().IsFile(Arg.Any<string>());
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
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

            var result = (RedirectToRouteResult)Controller.Upload("path", false, upload);

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

            var result = (RedirectToRouteResult)Controller.Upload("path", true, upload);

            FileRepository.Received().Save(Arg.Is<Stream>(stream), Arg.Is("path"), Arg.Is(true));

            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
        }

        [Test]
        public void CreateDirectory()
        {
            var result = (RedirectToRouteResult)Controller.CreateDirectory("path", string.Empty);

            FileRepository.Received().CreateDirectory(Arg.Is("path"), Arg.Any<string>());
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
        }

        [Test]
        public void Delete()
        {
            var result = (RedirectToRouteResult)Controller.Delete("path", "file");

            FileRepository.Received().Delete(Arg.Is("file"));
            Assert.That(result.RouteValues["Action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["path"], Is.EqualTo("path"));
        }

        [Test]
        public void RenderExistingFile()
        {
            FileRepository.IsFile(Arg.Is("file")).Returns(true);

            MimeTypeLookup.GetMimeType(Arg.Any<string>()).Returns("mime-type");

            var result = (FilePathResult)Controller.Render("file");

            FileRepository.Received().IsFile(Arg.Is("file"));
            MimeTypeLookup.Received().GetMimeType(Arg.Any<string>());

            Assert.That(result.FileName, Is.EqualTo("file"));
            Assert.That(result.ContentType, Is.EqualTo("mime-type"));
        }

        [Test]
        public void Return404OnMissingFile()
        {
            FileRepository.IsFile(Arg.Is("file")).Returns(false);

            var result = Controller.Render("file");

            FileRepository.Received().IsFile(Arg.Is("file"));
            MimeTypeLookup.DidNotReceive().GetMimeType(Arg.Any<string>());

            Assert.IsInstanceOf(typeof (HttpNotFoundResult), result);
        }
    }
}

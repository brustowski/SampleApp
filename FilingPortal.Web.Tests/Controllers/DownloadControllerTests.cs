using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class DownloadControllerTests : ApiControllerFunctionTestsBase<DownloadDocumentsController>
    {
        private Mock<IRailFilingHeadersRepository> _filingHeadersRepositoryMock;
        private Mock<IFileGenerator<RailFilingHeader>> _manifestPdfGeneratorMock;
        private Mock<IDocumentRepository<RailDocument>> _documentRepositoryMock;

        protected override DownloadDocumentsController TestInitialize()
        {
            _filingHeadersRepositoryMock = new Mock<IRailFilingHeadersRepository>();
            _manifestPdfGeneratorMock = new Mock<IFileGenerator<RailFilingHeader>>();
            _documentRepositoryMock = new Mock<IDocumentRepository<RailDocument>>();

            return new DownloadDocumentsController(_filingHeadersRepositoryMock.Object, _manifestPdfGeneratorMock.Object, _documentRepositoryMock.Object);
        }

        [TestMethod]
        public void DownloadManifest_CallsGenerator_WithRailFilingHeaderFromRepository()
        {
            var filingHeaderId = 34;

            var railFilingHeader = new RailFilingHeader();
            _filingHeadersRepositoryMock.Setup(x => x.GetRailFilingHeaderWithDetails(filingHeaderId)).Returns(railFilingHeader);
            var binaryFileModel = new BinaryFileModel { Content = new byte[] { 1 }, FileName = "A" };
            _manifestPdfGeneratorMock.Setup(x => x.Generate(railFilingHeader)).Returns(binaryFileModel);

            HttpResponseMessage result = Controller.DownloadManifest(filingHeaderId);

            _manifestPdfGeneratorMock.Verify(x => x.Generate(railFilingHeader), Times.Once);
        }


        [TestMethod]
        public void DownloadDocument_CallsRepository()
        {
            var docId = 23;
            var doc = new RailDocument { Id = docId, Content = new byte[] { 1, 2, 3 }, FileName = "doc name" };
            _documentRepositoryMock.Setup(x => x.Get(docId)).Returns(doc);

            Controller.DownloadDocument(docId);

            _documentRepositoryMock.Verify(x => x.Get(docId), Times.Once);
        }

        [TestMethod]
        public void DownloadDocument_ReturnsFile()
        {
            var docId = 23;
            var doc = new RailDocument { Id = docId, Content = new byte[] { 1, 2, 3 }, FileName = "doc name.jpg" };
            _documentRepositoryMock.Setup(x => x.Get(docId)).Returns(doc);

            HttpResponseMessage result = Controller.DownloadDocument(docId);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(result.Content);
            Assert.IsTrue(result.Content is StreamContent);
            Assert.AreEqual("image/jpeg", result.Content.Headers.ContentType.MediaType);
            Assert.AreEqual("\"" + doc.FileName + "\"", result.Content.Headers.ContentDisposition.FileName);
        }
    }
}
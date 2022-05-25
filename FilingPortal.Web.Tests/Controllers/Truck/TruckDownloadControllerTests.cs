using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckDownloadControllerTests : ApiControllerFunctionTestsBase<TruckDownloadDocumentsController>
    {
        private Mock<IRailFilingHeadersRepository> _filingHeadersRepositoryMock;
        private Mock<IDocumentRepository<TruckDocument>> _documentRepositoryMock;

        protected override TruckDownloadDocumentsController TestInitialize()
        {
            _filingHeadersRepositoryMock = new Mock<IRailFilingHeadersRepository>();
            _documentRepositoryMock = new Mock<IDocumentRepository<TruckDocument>>();

            return new TruckDownloadDocumentsController(_filingHeadersRepositoryMock.Object, _documentRepositoryMock.Object);
        }

        [TestMethod]
        public void DownloadDocument_CallsRepository()
        {
            var docId = 23;
            var doc = new TruckDocument { Id = docId, Extension = "image/jpeg", Content = new byte[] { 1, 2, 3 }, FileName = "doc name" };
            _documentRepositoryMock.Setup(x => x.Get(docId)).Returns(doc);

            Controller.DownloadDocument(docId);

            _documentRepositoryMock.Verify(x => x.Get(docId), Times.Once);
        }

        [TestMethod]
        public void DownloadDocument_ReturnsFile()
        {
            var docId = 23;
            var doc = new TruckDocument { Id = docId, Content = new byte[] { 1, 2, 3 }, FileName = "doc name.jpg" };
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
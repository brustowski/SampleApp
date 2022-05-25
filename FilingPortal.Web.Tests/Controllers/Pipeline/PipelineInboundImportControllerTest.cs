using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.Web.Controllers.Pipeline;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Pipeline
{
    [TestClass]
    public class PipelineInboundImportControllerTest : ApiControllerFunctionTestsBase<PipelineInboundImportController>
    {
        private Mock<IPipelineInboundExcelFileImportService> _importServiceMock;

        protected override PipelineInboundImportController TestInitialize()
        {
            _importServiceMock = new Mock<IPipelineInboundExcelFileImportService>();
            return new PipelineInboundImportController(_importServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProcessFile_ThrowHttpResponseException_WithEmptyRequestContent()
        {
            await Controller.ProcessFile(Controller.Request);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProcessFile_ThrowHttpResponseException_NotMultipartRequestMimeType()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("Test")
            };
            await Controller.ProcessFile(request);
        }

        [TestMethod]
        public async Task ProcessFile_ReturnBadRequest_RequestWithoutFiles()
        {
            var request = new HttpRequestMessage
            {
                Content = new MultipartFormDataContent("test-boundary")
            };

            var result = await Controller.ProcessFile(request);

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }
    }
}

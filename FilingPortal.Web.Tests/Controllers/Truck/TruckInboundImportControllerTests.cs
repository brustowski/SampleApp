using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Services.Truck;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.Models;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckInboundImportControllerTests : ApiControllerFunctionTestsBase<TruckInboundImportController>
    {
        private Mock<ITruckInboundExcelFileImportService> _importServiceMock;

        protected override TruckInboundImportController TestInitialize()
        {
            _importServiceMock = new Mock<ITruckInboundExcelFileImportService>();
            return new TruckInboundImportController(_importServiceMock.Object);
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

        [TestMethod]
        public async Task ProcessFile_ReturnOk_WithValidRequest()
        {
            var fileContent = Encoding.UTF8.GetBytes("test string");

            var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture))
            {
                { new  ByteArrayContent(fileContent), "testFile", "testfile.xlsx" }
            };

            var request = new HttpRequestMessage
            {
                Content = content
            };

            var result = await Controller.ProcessFile(request);

            Assert.IsTrue(result is OkNegotiatedContentResult<FileProcessingResultViewModel<ExcelFileValidationError>>);
        }
    }
}

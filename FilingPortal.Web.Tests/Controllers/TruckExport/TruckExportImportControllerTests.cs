using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Controllers.TruckExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Web.Tests.Controllers.TruckExport
{
    [TestClass]
    public class TruckExportImportControllerTests : ApiControllerFunctionTestsBase<TruckExportImportController>
    {
        private Mock<ITruckExportExcelFileImportService> _importServiceMock;


        protected override TruckExportImportController TestInitialize()
        {
            _importServiceMock = new Mock<ITruckExportExcelFileImportService>();
            return new TruckExportImportController(_importServiceMock.Object);
        }


        private static HttpRequestMessage GenerateRequest()
        {
            var fileContent = Encoding.UTF8.GetBytes("test string");

            var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture))
            {
                {new ByteArrayContent(fileContent), "testFile", "testFile.xlsx"}
            };

            var request = new HttpRequestMessage
            {
                Content = content
            };
            return request;
        }

        [TestMethod]
        public void ImportFromFile_RouteShouldBeExist()
        {
            const string route = "/api/export/truck/import";
            AssertRoute(HttpMethod.Post, route, x => x.ImportFromFile(null));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProcessFile_ThrowHttpResponseException_WithEmptyRequestContent()
        {
            await Controller.ImportFromFile(Controller.Request);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task ProcessFile_ThrowHttpResponseException_NotMultipartRequestMimeType()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("Test")
            };

            await Controller.ImportFromFile(request);
        }

        [TestMethod]
        public async Task ProcessFile_ReturnBadRequest_RequestWithoutFiles()
        {
            var request = new HttpRequestMessage
            {
                Content = new MultipartFormDataContent("test-boundary")
            };

            IHttpActionResult result = await Controller.ImportFromFile(request);

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }

        [TestMethod]
        public async Task ProcessFile_ReturnOk_WithValidRequest()
        {
            HttpRequestMessage request = GenerateRequest();

            IHttpActionResult result = await Controller.ImportFromFile(request);

            Assert.IsTrue(result is OkNegotiatedContentResult<FileProcessingResultViewModel<ExcelFileValidationError>>);
        }
    }
}

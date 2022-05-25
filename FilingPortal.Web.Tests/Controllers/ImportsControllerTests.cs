using FilingPortal.Domain;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Import.TemplateEngine;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ImportsControllerTests : ApiControllerFunctionTestsBase<ImportsController>
    {
        private Mock<IImportConfigurationRegistry> _registryMock;
        private Mock<IImportConfiguration> _configurationMock;
        private Mock<ITemplateService> _serviceMock;
        private Mock<ITemplateProcessingService> _templateProcessingServiceMock;
        private Mock<ITemplateProcessingServiceFactory> _factoryMock;
        private Mock<IAppDocumentRepository> _documentRepositoryMock;
        private Mock<IFormDataTemplateProcessingService> _formDataTemplateProcessingService;

        protected override ImportsController TestInitialize()
        {
            _configurationMock = new Mock<IImportConfiguration>();
            _configurationMock.Setup(x => x.Permissions).Returns(new[] { 1, 2, 3 });
            _registryMock = new Mock<IImportConfigurationRegistry>();
            _registryMock.Setup(x => x.GetConfiguration(It.IsAny<string>())).Returns(_configurationMock.Object);
            _serviceMock = new Mock<ITemplateService>();
            _serviceMock.Setup(x => x.Create(It.IsAny<IImportConfiguration>())).Returns(new FileExportResult());
            _templateProcessingServiceMock = new Mock<ITemplateProcessingService>();
            _templateProcessingServiceMock.Setup(x => x.Verify(It.IsAny<AppDocument>()))
                .Returns(new FileProcessingDetailedResult("test_file_name"));
            _factoryMock = new Mock<ITemplateProcessingServiceFactory>();
            _factoryMock.Setup(x => x.Create(It.IsAny<IImportConfiguration>()))
                .Returns(_templateProcessingServiceMock.Object);
            _documentRepositoryMock = new Mock<IAppDocumentRepository>();
            _documentRepositoryMock.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);
            _formDataTemplateProcessingService = new Mock<IFormDataTemplateProcessingService>();
            return new ImportsController(_registryMock.Object, _serviceMock.Object, _factoryMock.Object, _documentRepositoryMock.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Get, "/api/imports/templates/test", x => x.GetTemplateByName("test"));
            AssertRoute(HttpMethod.Post, "/api/imports/upload/grids/test", x => x.ProcessFile(null, "test"));
            AssertRoute(HttpMethod.Post, "/api/imports/uploads", x => x.UploadFile(null));
            AssertRoute(HttpMethod.Get, "/api/imports/uploads/1/test", x => x.CheckUploadedFile("1", "test"));
            AssertRoute(HttpMethod.Post, "/api/imports/uploads/1/test", x => x.ImportDataFromUploadedFile("1", "test"));
            AssertRoute(HttpMethod.Delete, "/api/imports/uploads/1", x => x.DeleteUploadedFile("1"));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetByTemplateName_CallsRegistry_GetConfiguration()
        {
            const string templateName = "test";

            Controller.GetTemplateByName(templateName);

            _registryMock.Verify(x => x.GetConfiguration(templateName), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetByTemplateName_CallsService_Create()
        {
            const string templateName = "test";

            Controller.GetTemplateByName(templateName);

            _serviceMock.Verify(x => x.Create(It.IsAny<IImportConfiguration>()), Times.Once);
        }

        [TestMethod]
        public void GetByTemplateName_ReturnsNotFound_IfConfigurationNotFound()
        {
            const string templateName = "test";

            _registryMock.Setup(x => x.GetConfiguration(templateName)).Returns<IImportConfiguration>(null);

            HttpResponseMessage result = Controller.GetTemplateByName(templateName);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual($"The template with '{templateName}' was not found.", ((ObjectContent<string>)result.Content).Value);
        }

        #region Upload file
        [TestMethod]
        public async Task UploadFile_CallsAdd_AppDocumentRepository()
        {
            const string fileContent = "File content";
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContent));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "test.xlsx"
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
                ,
                Content = new MultipartFormDataContent() { content }
            };

            await Controller.UploadFile(request);

            _documentRepositoryMock.Verify(x => x.Add(It.IsAny<AppDocument>()), Times.Once);
        }

        [TestMethod]
        public async Task UploadFile_CallsSave_AppDocumentRepository()
        {
            const string fileContent = "File content";
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContent));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "test.xlsx"
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new MultipartFormDataContent() { content }
            };

            await Controller.UploadFile(request);

            _documentRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [TestMethod]
        public async Task UploadFile_ReturnsOk_IfDocumentSuccessfullyUploaded()
        {
            const string fileContent = "File content";
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContent));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "test.xlsx"
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
                ,
                Content = new MultipartFormDataContent() { content }
            };

            IHttpActionResult a = await Controller.UploadFile(request);

            Assert.IsInstanceOfType(a, typeof(OkNegotiatedContentResult<int>));
        }
        #endregion

        #region Delete uploaded file
        [TestMethod]
        public void DeleteUploadedFile_ReturnsOk_IfDocumentSuccessfullyDeleted()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("1");

            Assert.IsInstanceOfType(a, typeof(OkResult));
        }

        [TestMethod]
        public void DeleteUploadedFile_CallsDelete_AppDocumentRepository()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("1");

            _documentRepositoryMock.Verify(x => x.Delete(1), Times.Once);
        }

        [TestMethod]
        public void DeleteUploadedFile_CallsIsExist_AppDocumentRepository()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("1");

            _documentRepositoryMock.Verify(x => x.IsExist(1), Times.Once);
        }

        [TestMethod]
        public void DeleteUploadedFile_ReturnsBadRequest_IfIdFormatIsWrong()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("abc");

            Assert.IsInstanceOfType(a, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document identifier format mismatch", (a as BadRequestErrorMessageResult)?.Message);
        }

        [TestMethod]
        public void DeleteUploadedFile_ReturnsBadRequest_IfDocumentNotFound()
        {
            _documentRepositoryMock.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            IHttpActionResult a = Controller.DeleteUploadedFile("1");

            Assert.IsInstanceOfType(a, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document with specified identifier is not found", (a as BadRequestErrorMessageResult)?.Message);
        }
        #endregion

        #region Check Uploaded File
        [TestMethod]
        public void CheckUploadedFile_ReturnsBadRequest_IfIdFormatIsWrong()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("abc");

            Assert.IsInstanceOfType(a, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document identifier format mismatch", (a as BadRequestErrorMessageResult)?.Message);
        }

        [TestMethod]
        public void CheckUploadedFile_ReturnsBadRequest_IfDocumentNotFound()
        {
            _documentRepositoryMock.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            IHttpActionResult result = Controller.DeleteUploadedFile("1");

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document with specified identifier is not found", (result as BadRequestErrorMessageResult)?.Message);
        }

        [TestMethod]
        public void CheckUploadedFile_CallsIsExist_AppDocumentRepository()
        {
            const string gridName = "test_grid";
            Controller.CheckUploadedFile("1", gridName);

            _documentRepositoryMock.Verify(x => x.IsExist(1), Times.Once);
        }

        [TestMethod]
        public void CheckUploadedFile_CallsGetConfiguration_ConfigurationRegistry()
        {
            const string gridName = "test_grid";
            Controller.CheckUploadedFile("1", gridName);

            _registryMock.Verify(x => x.GetConfiguration(gridName), Times.Once);
        }

        [TestMethod]
        public void CheckUploadedFile_ReturnsForbidden_IfUserDoesNotHavePermissions()
        {
            const string gridName = "test_grid";

            _configurationMock.Setup(x => x.Permissions).Returns(new[] { 999 });

            var result = Controller.CheckUploadedFile("1", gridName) as ResponseMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Forbidden, result.Response.StatusCode);
            var content = result.Response.Content as ObjectContent<HttpError>;
            Assert.AreEqual(ErrorMessages.InsufficientPermissions, (content.Value as HttpError).Message);
        }

        [TestMethod]
        public void CheckUploadedFile_CallsCreate_ProcessingFactory()
        {
            const string gridName = "test_grid";
            Controller.CheckUploadedFile("1", gridName);

            _factoryMock.Verify(x => x.Create(_configurationMock.Object), Times.Once);
        }

        [TestMethod]
        public void CheckUploadedFile_CallsGetDocument_AppDocumentRepository()
        {
            const string gridName = "test_grid";
            Controller.CheckUploadedFile("1", gridName);

            _documentRepositoryMock.Verify(x => x.GetDocument(1), Times.Once);
        }

        [TestMethod]
        public void CheckUploadedFile_CallsVerify_TemplateProcessingService()
        {
            const string gridName = "test_grid";
            Controller.CheckUploadedFile("1", gridName);

            _templateProcessingServiceMock.Verify(x => x.Verify(It.IsAny<AppDocument>()), Times.Once);
        }

        [TestMethod]
        public void CheckUploadedFile_ReturnsValidationResult()
        {
            IHttpActionResult result = Controller.CheckUploadedFile("1", "test_grid");

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<FileProcessingResultViewModel<ExcelFileValidationError>>));
        }
        #endregion

        #region Import data from Uploaded File
        [TestMethod]
        public void ImportDataFromUploadedFile_ReturnsBadRequest_IfIdFormatIsWrong()
        {
            IHttpActionResult a = Controller.DeleteUploadedFile("abc");

            Assert.IsInstanceOfType(a, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document identifier format mismatch", (a as BadRequestErrorMessageResult)?.Message);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_ReturnsBadRequest_IfDocumentNotFound()
        {
            _documentRepositoryMock.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            IHttpActionResult result = Controller.DeleteUploadedFile("1");

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Document with specified identifier is not found", (result as BadRequestErrorMessageResult)?.Message);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_CallsIsExist_AppDocumentRepository()
        {
            const string gridName = "test_grid";
            Controller.ImportDataFromUploadedFile("1", gridName);

            _documentRepositoryMock.Verify(x => x.IsExist(1), Times.Once);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_CallsGetConfiguration_ConfigurationRegistry()
        {
            const string gridName = "test_grid";
            Controller.ImportDataFromUploadedFile("1", gridName);

            _registryMock.Verify(x => x.GetConfiguration(gridName), Times.Once);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_ReturnsForbidden_IfUserDoesNotHavePermissions()
        {
            const string gridName = "test_grid";

            _configurationMock.Setup(x => x.Permissions).Returns(new[] { 999 });

            var result = Controller.ImportDataFromUploadedFile("1", gridName) as ResponseMessageResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Forbidden, result.Response.StatusCode);
            var content = result.Response.Content as ObjectContent<HttpError>;
            Assert.AreEqual(ErrorMessages.InsufficientPermissions, (content.Value as HttpError).Message);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_CallsCreate_ProcessingFactory()
        {
            const string gridName = "test_grid";
            Controller.ImportDataFromUploadedFile("1", gridName);

            _factoryMock.Verify(x => x.Create(_configurationMock.Object), Times.Once);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_CallsGetDocument_AppDocumentRepository()
        {
            const string gridName = "test_grid";
            Controller.ImportDataFromUploadedFile("1", gridName);

            _documentRepositoryMock.Verify(x => x.GetDocument(1), Times.Once);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_CallsVerify_TemplateProcessingService()
        {
            const string gridName = "test_grid";
            Controller.ImportDataFromUploadedFile("1", gridName);

            _templateProcessingServiceMock.Verify(x => x.Import(It.IsAny<AppDocument>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ImportDataFromUploadedFile_ReturnsValidationResult()
        {
            IHttpActionResult result = Controller.ImportDataFromUploadedFile("1", "test_grid");

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<FileProcessingResultViewModel<ExcelFileValidationError>>));
        }
        #endregion
    }
}

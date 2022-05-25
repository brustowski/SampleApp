using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ReportExportControllerTests : ApiControllerFunctionTestsBase<ReportExportController>
    {
        private Mock<IReportExportingService> _exportingServiceMock;
        protected override ReportExportController TestInitialize()
        {
            _exportingServiceMock = new Mock<IReportExportingService>();

            return new ReportExportController(_exportingServiceMock.Object);
        }

        [TestMethod]
        public async Task ExportToExcel_WhenCalled_CallsServiceToCreateReportWithDeserializedData()
        {
            var serializedData = "eyJQYWdpbmdTZXR0aW5ncyI6eyJQYWdlTnVtYmVyIjoxLCJQYWdlU2l6ZSI6NTB9LCJTb3J0aW5nU2V0dGluZ3MiOnsiRmllbGQiOiJJbXBvcnRlciIsIlNvcnRPcmRlciI6ImFzYyJ9LCJGaWx0ZXJTZXR0aW5ncyI6eyJGaWx0ZXJzIjpbeyJGaWVsZE5hbWUiOiJJbXBvcnRlciIsIk9wZXJhbmQiOiJjb250YWlucyIsIlZhbHVlcyI6W3siVmFsdWUiOiJhIiwiRGlzcGxheVZhbHVlIjoiYSJ9XX1dfX0=";
            _exportingServiceMock.Setup(x => x.GetExportingResult("grid",
                It.IsAny<SearchRequestModel>())).Returns(Task.FromResult(new FileExportResult { FileName = "exported_file", DocumentExternalName = "exported_file" }));

            await Controller.ExportToExcel("grid", serializedData);

            _exportingServiceMock.Verify(x => x.GetExportingResult("grid",
                It.Is<SearchRequestModel>(s =>
                    s.SortingSettings.Field == "Importer" &&
                    s.SortingSettings.SortOrder == SortOrder.Asc &&
                    s.FilterSettings.Filters.First().FieldName == "Importer" &&
                    s.FilterSettings.Filters.First().Operand == "contains" &&
                    (string)s.FilterSettings.Filters.First().Values.First().Value == "a"
                )));
        }

        [TestMethod]
        public async Task ExportToExcel_WhenCalled_ReturnsFileContentResult()
        {
            var serializedData = "eyJQYWdpbmdTZXR0aW5ncyI6eyJQYWdlTnVtYmVyIjoxLCJQYWdlU2l6ZSI6NTB9LCJTb3J0aW5nU2V0dGluZ3MiOnsiRmllbGQiOiJJbXBvcnRlciIsIlNvcnRPcmRlciI6ImFzYyJ9LCJGaWx0ZXJTZXR0aW5ncyI6eyJGaWx0ZXJzIjpbeyJGaWVsZE5hbWUiOiJJbXBvcnRlciIsIk9wZXJhbmQiOiJjb250YWlucyIsIlZhbHVlcyI6W3siVmFsdWUiOiJhIiwiRGlzcGxheVZhbHVlIjoiYSJ9XX1dfX0=";
            var docResult = new FileExportResult { FileName = "exported_file", DocumentExternalName = "exported_file" };
            _exportingServiceMock.Setup(x => x.GetExportingResult("grid",
                It.IsAny<SearchRequestModel>())).Returns(Task.FromResult(docResult));

            FileExportResult result = await Controller.ExportToExcel("grid", serializedData);

            Assert.IsNotNull(result);
            Assert.AreEqual(docResult.FileName, result.FileName);
            Assert.AreEqual(docResult.DocumentExternalName, result.DocumentExternalName);
        }
    }
}
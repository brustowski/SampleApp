using System;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Services.GridExport.Models;
using Framework.Domain.Paging;
using Framework.Infrastructure;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Common.Reporting
{
    [TestClass]
    public class ReportExportingServiceTests
    {
        #region Setup

        private static readonly SearchRequestModel SearchRequestModel = new SearchRequestModel();

        private ReportExportingService _ReportExportingService;

        private Mock<IReportingService> _reportingServiceMock;
        private Mock<IReportConfigRegistry> _reportConfigRegistryMock;

        [TestInitialize]
        public void Init()
        {
            _reportingServiceMock = new Mock<IReportingService>();
            _reportConfigRegistryMock = new Mock<IReportConfigRegistry>();

            _ReportExportingService = new ReportExportingService(_reportingServiceMock.Object, _reportConfigRegistryMock.Object);
        }

        #endregion


        [TestMethod]
        public async Task GetExportingResult_WhenGridNameIsUnsupported_ThrowNullReferenceException()
        {
            var gridName = "__unsupported__";
            var reportConfig = MakeReportConfig<RailInboundRecordsReportModel>(gridName);
            _reportConfigRegistryMock.Setup(x => x.GetConfig(gridName)).Returns(reportConfig);
            Task action() => _ReportExportingService.GetExportingResult(gridName, SearchRequestModel);

            await AssertThat.ThrowsAsync<NullReferenceException>(action);
        }

        private static ReportConfig<T> MakeReportConfig<T>(string gridName) where T: class
        {
            return new ReportConfig<T>(gridName);
        }

        [TestMethod]
        public async Task GetExportingResult_Call_GetConfigFromRegistry()
        {
            var gridName = GridNames.InboundRecords;
            var reportConfig = MakeReportConfig<RailInboundRecordsReportModel>(gridName);
            _reportConfigRegistryMock.Setup(x => x.GetConfig(gridName)).Returns(reportConfig);
            _reportingServiceMock.Setup(x => x.ExportToFile<RailInboundRecordsReportModel, RailInboundRecordsReportModel>(reportConfig, SearchRequestModel))
                .Returns(Task.FromResult(new FileExportResult { FileName = "test.txt", DocumentExternalName = "test document" }));

            await _ReportExportingService.GetExportingResult(gridName, SearchRequestModel);

            _reportConfigRegistryMock.VerifyOnce(x => x.GetConfig(gridName));
        }

        [TestMethod]
        public async Task GetExportingResult_InboundRecordsGridName_CallExportToFile()
        {
            var gridName = GridNames.InboundRecords;
            var reportConfig = MakeReportConfig<RailInboundRecordsReportModel>(gridName);
            _reportConfigRegistryMock.Setup(x => x.GetConfig(gridName)).Returns(reportConfig);
            _reportingServiceMock.Setup(x => x.ExportToFile<RailInboundRecordsReportModel, RailInboundRecordsReportModel>(reportConfig, SearchRequestModel))
                .Returns(Task.FromResult(new FileExportResult { FileName = "test.txt", DocumentExternalName = "test document" }));

            await _ReportExportingService.GetExportingResult(gridName, SearchRequestModel);

            _reportingServiceMock.VerifyOnce(x => x.ExportToFile<RailInboundRecordsReportModel, RailInboundRecordsReportModel>(reportConfig, SearchRequestModel));
        }
    }
}

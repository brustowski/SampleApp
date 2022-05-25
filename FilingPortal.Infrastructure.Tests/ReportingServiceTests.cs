using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Infrastructure.Services;
using Framework.Domain.Paging;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilingPortal.Infrastructure.Tests
{
    [TestClass]
    public class ReportingServiceTests
    {
        #region Setup

        private ReportingService _reportingService;

        private Mock<IReporterFactory> _excelReporterFactoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IReportDataSourceResolver> _reportDataSourceResolverMock;
        private Mock<IReportFiltersBuilder> _reportFiltersBuilderMock;
        private Mock<IReportBodyBuilder> _reportBodyBuilderMock;
        private Mock<IReporter> _excelReporterMock;
        private Mock<IReportDatasource> _reportDataSourceMock;
        private const string SavedFilePath = "savedFilePath";

        private ReportConfig<RailInboundReadModel> _reportConfig;
        private SearchRequestModel _searchRequestModel;
        private SearchRequest _searchRequest;
        private IEnumerable<RailInboundReadModel> _reportDataSourceResult;

        [TestInitialize]
        public void Init()
        {
            _excelReporterFactoryMock = new Mock<IReporterFactory>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _reportDataSourceResolverMock = new Mock<IReportDataSourceResolver>();
            _reportFiltersBuilderMock = new Mock<IReportFiltersBuilder>();
            _reportBodyBuilderMock = new Mock<IReportBodyBuilder>();
            _excelReporterMock = new Mock<IReporter>();
            _reportDataSourceMock = new Mock<IReportDatasource>();
            _reportingService = new ReportingService(
                _excelReporterFactoryMock.Object,
                _searchRequestFactoryMock.Object,
                _reportDataSourceResolverMock.Object,
                _reportFiltersBuilderMock.Object,
                _reportBodyBuilderMock.Object);

            _reportConfig = new ReportConfig<RailInboundReadModel>(GridNames.InboundRecords)
            {
                DocumentTitle = "DocumentTitle",
                FileName = "FileName"
            };
            _searchRequestModel = new SearchRequestModel();
            _searchRequest = new SearchRequest();
            _reportDataSourceResult = Enumerable.Empty<RailInboundReadModel>();

            _excelReporterFactoryMock.Setup(x => x.Create(_reportConfig.FileName)).Returns(_excelReporterMock.Object);
            _reportFiltersBuilderMock.Setup(x => x.GetRows(_searchRequestModel.FilterSettings.Filters)).Returns(new Row[0]);
            _reportBodyBuilderMock.Setup(x => x.GetHeaderRow<RailInboundReadModel>()).Returns(new Row());
            _reportDataSourceMock.Setup(x => x.GetAllAsync<RailInboundReadModel>(_searchRequest)).Returns(Task.FromResult(_reportDataSourceResult));
            _reportDataSourceResolverMock.Setup(x => x.Resolve(It.IsAny<string>())).Returns(_reportDataSourceMock.Object);
            _reportBodyBuilderMock.Setup(x => x.GetRows(_reportDataSourceResult)).Returns(new Row[0]);
            _excelReporterMock.Setup(x => x.SaveToFile()).Returns(SavedFilePath);
        }

        #endregion


        [TestMethod]
        public async Task ExportToFile_Call_ResultPropertiesAreInitialized()
        {
            // Act
            FileExportResult result = await _reportingService.ExportToFile<RailInboundReadModel, RailInboundReadModel>(_reportConfig, _searchRequestModel);

            // Assert
            result.FileName.ShouldBeEqualTo(SavedFilePath);
            result.DocumentExternalName.ShouldBeEqualTo(_reportConfig.FileName);
        }

    }
}

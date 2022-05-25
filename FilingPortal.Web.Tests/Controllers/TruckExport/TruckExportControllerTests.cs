using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.TruckExport;
using FilingPortal.Web.Models.TruckExport;
using FilingPortal.Web.PageConfigs.TruckExport;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Web.Tests.Controllers.TruckExport
{
    [TestClass]
    public class TruckExportControllerTests : ApiControllerFunctionTestsBase<TruckExportController>
    {
        private Mock<ITruckExportReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IListInboundValidator<TruckExportReadModel>> _selectedInboundRecordValidatorMock;
        private Mock<ISingleRecordValidator<TruckExportReadModel>> _singleInboundRecordValidatorMock;
        private Mock<IFilingHeaderDocumentUpdateService<TruckExportDocumentDto>> _filingHeaderDocumentUpdateServiceMock;
        private Mock<ISpecificationBuilder> _specificationBuilder;
        private Mock<ITruckExportRepository> _truckExportsRepository;
        private Mock<ITruckExportJobNumberService> _jobNumberServiceMock;

        protected override TruckExportController TestInitialize()
        {
            _repositoryMock = new Mock<ITruckExportReadModelRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<ICommand>())).Returns(CommandResult.Ok);
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _selectedInboundRecordValidatorMock = new Mock<IListInboundValidator<TruckExportReadModel>>();
            _singleInboundRecordValidatorMock = new Mock<ISingleRecordValidator<TruckExportReadModel>>();
            _filingHeaderDocumentUpdateServiceMock = new Mock<IFilingHeaderDocumentUpdateService<TruckExportDocumentDto>>();
            _specificationBuilder = new Mock<ISpecificationBuilder>();
            _truckExportsRepository = new Mock<ITruckExportRepository>();
            _jobNumberServiceMock = new Mock<ITruckExportJobNumberService>();

            return new TruckExportController(
                _repositoryMock.Object,
                _searchRequestFactoryMock.Object,
                _commandDispatcherMock.Object,
                _pageConfigContainerMock.Object,
                _selectedInboundRecordValidatorMock.Object,
                _singleInboundRecordValidatorMock.Object,
                _filingHeaderDocumentUpdateServiceMock.Object,
                _specificationBuilder.Object,
                _truckExportsRepository.Object,
                _jobNumberServiceMock.Object
                );
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/export/truck/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/search", x => x.Search(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/delete", x => x.Delete(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/validate-selected-records", x => x.ValidateSelectedRecords(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/documents-upload", x => x.ProcessFile(null));
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<TruckExportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<TruckExportReadModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<TruckExportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckExportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckExportReadModel>() { CurrentPage = 1, Results = new List<TruckExportReadModel>() });

            SimplePagedResult<TruckExportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public async Task Search_CallsValidationForEachRecord_WhenCalled()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord1 = new TruckExportReadModel { Exporter = "Exporter" };
            var inboundRecord2 = new TruckExportReadModel { Exporter = "Exporter" };
            _searchRequestFactoryMock.Setup(x => x.Create<TruckExportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckExportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckExportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckExportReadModel>
                        {
                            inboundRecord1,
                            inboundRecord2
                        }
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckExportActionsConfig());

            await Controller.Search(searchRequestModel);
            _singleInboundRecordValidatorMock.Verify(x => x.GetErrors(It.IsAny<TruckExportReadModel>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Search_ReturnsEmptyErrorListForItemAndHighlightType_WhenItemHasNoErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new TruckExportReadModel { Exporter = "Exporter" };
            _searchRequestFactoryMock.Setup(x => x.Create<TruckExportReadModel>(searchRequestModel))
                .Returns(searchRequest);
            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckExportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckExportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckExportReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckExportActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<TruckExportReadModel>())).Returns(new List<string>());

            SimplePagedResult<TruckExportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(0, result.Results.First().Errors.Count);
            Assert.AreEqual(HighlightingType.NoHighlighting, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public async Task Search_ReturnsErrorListForItemAndErrorHighlighting_WhenItemHasErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new TruckExportReadModel
            {
                Exporter = "Exporter"
            };
            _searchRequestFactoryMock.Setup(x => x.Create<TruckExportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckExportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckExportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckExportReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckExportActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<TruckExportReadModel>())).Returns(new List<string> { "error 1" });

            SimplePagedResult<TruckExportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.Results.First().Errors.Count);
            Assert.AreEqual("error 1", result.Results.First().Errors.ElementAt(0));
            Assert.AreEqual(HighlightingType.Error, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public void Delete_WhenCalled_DispatchDeleteCommand()
        {
            var recordIds = new[] { 423 };

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<ICommand>())).Returns(CommandResult.Ok);

            Controller.Delete(recordIds);

            _commandDispatcherMock.Verify(x =>
                x.Dispatch(It.Is<TruckExportMassDeleteCommand>(c => c.RecordIds == recordIds)));
        }

        private static string ToBase64String<T>(T data)
        {
            var so = JsonConvert.SerializeObject(data);
            var ba = Encoding.UTF8.GetBytes(so);
            return Convert.ToBase64String(ba);
        }
    }
}

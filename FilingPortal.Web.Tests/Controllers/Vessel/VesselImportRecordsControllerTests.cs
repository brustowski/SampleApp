using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Vessel;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Vessel
{
    [TestClass()]
    public class VesselImportRecordsControllerTests : ApiControllerFunctionTestsBase<VesselImportRecordsController>
    {
        private Mock<IVesselImportReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IListInboundValidator<VesselImportReadModel>> _selectedInboundRecordValidatorMock;
        private Mock<ISingleRecordValidator<VesselImportReadModel>> _singleInboundRecordValidatorMock;
        private Mock<IVesselImportRepository> _importsRepositoryMock;

        protected override VesselImportRecordsController TestInitialize()
        {
            _repositoryMock = new Mock<IVesselImportReadModelRepository>();
            _importsRepositoryMock = new Mock<IVesselImportRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _selectedInboundRecordValidatorMock = new Mock<IListInboundValidator<VesselImportReadModel>>();
            _singleInboundRecordValidatorMock = new Mock<ISingleRecordValidator<VesselImportReadModel>>();
            return new VesselImportRecordsController(_repositoryMock.Object, _importsRepositoryMock.Object, _searchRequestFactoryMock.Object, _commandDispatcherMock.Object,
                _pageConfigContainerMock.Object, _selectedInboundRecordValidatorMock.Object, _singleInboundRecordValidatorMock.Object);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<VesselImportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<VesselImportReadModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<VesselImportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<VesselImportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<VesselImportReadModel>() { CurrentPage = 1, Results = new List<VesselImportReadModel>() });

            SimplePagedResult<VesselImportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public async Task Search_CallsValidationForEachRecord_WhenCalled()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord1 = new VesselImportReadModel { ImporterCode = "I", Vessel = "P" };
            var inboundRecord2 = new VesselImportReadModel { ImporterCode = "I", Vessel = "P" };
            _searchRequestFactoryMock.Setup(x => x.Create<VesselImportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<VesselImportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<VesselImportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<VesselImportReadModel>
                        {
                            inboundRecord1,
                            inboundRecord2
                        }
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new VesselImportActionsConfig());

            await Controller.Search(searchRequestModel);
            _singleInboundRecordValidatorMock.Verify(x => x.GetErrors(It.IsAny<VesselImportReadModel>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Search_ReturnsEmptyErrorListForItemAndHighlightType_WhenItemHasNoErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new VesselImportReadModel { ImporterCode = "I", Vessel = "P" };
            _searchRequestFactoryMock.Setup(x => x.Create<VesselImportReadModel>(searchRequestModel))
                .Returns(searchRequest);
            _repositoryMock.Setup(x => x.GetAllOrderedAsync<VesselImportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<VesselImportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<VesselImportReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new VesselImportActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<VesselImportReadModel>())).Returns(new List<string>());

            SimplePagedResult<VesselImportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(0, result.Results.First().Errors.Count);
            Assert.AreEqual(HighlightingType.NoHighlighting, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public async Task Search_ReturnsErrorListForItemAndErrorHighlighting_WhenItemHasErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new VesselImportReadModel
            {
                ImporterCode = "importer"
            };
            _searchRequestFactoryMock.Setup(x => x.Create<VesselImportReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<VesselImportReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<VesselImportReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<VesselImportReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new VesselImportActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<VesselImportReadModel>())).Returns(new List<string> { "error 1" });

            SimplePagedResult<VesselImportViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.Results.First().Errors.Count);
            Assert.AreEqual("error 1", result.Results.First().Errors.ElementAt(0));
            Assert.AreEqual(HighlightingType.Error, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public void Delete_WhenCalled_DispatchDeleteCommand()
        {
            var recordIds = new int[] { 423 };

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<ICommand>())).Returns(CommandResult.Ok);

            Controller.Delete(recordIds);

            _commandDispatcherMock.Verify(x =>
                x.Dispatch(It.Is<VesselImportMassDeleteCommand>(c => c.RecordIds == recordIds)));
        }

        [TestMethod]
        public void GetAvailableActions_ReturnsRecordsAvailableActios()
        {
            var ids = new List<int>() { 3, 4 };
            var records = new List<VesselImportReadModel> { new VesselImportReadModel() };
            _repositoryMock.Setup(x => x.GetList(ids)).Returns(records);
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new VesselImportListActionsConfig());

            Actions result = Controller.GetAvailableActions(ids);

            Assert.IsInstanceOfType(result, typeof(Actions));
        }

        [TestMethod]
        public void AddNewVessel_Returns_CommandResult()
        {
            var model = new Mock<VesselImportEditModel>();
            model.SetupGet(x => x.Eta).Returns("01/01/2019");
            model.Object.ImporterId = Guid.NewGuid().ToString();

            var commandResult = new CommandResult<int>(10);

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<VesselImportAddOrUpdateCommand>())).Returns(commandResult);

            ValidationResultWithFieldsErrorsViewModel<int?> result = Controller.AddNewVessel(model.Object);

            Assert.IsInstanceOfType(result, typeof(ValidationResultWithFieldsErrorsViewModel<int?>));
            Assert.AreEqual(result.Data, 10);
        }

        [TestMethod]
        public void EditInboundVessel_Returns_CommandResult()
        {
            var model = new Mock<VesselImportEditModel>();
            model.SetupGet(x => x.Eta).Returns("01/01/2019");
            model.Object.ImporterId = Guid.NewGuid().ToString();

            var commandResult = new CommandResult<int>(10);

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<VesselImportAddOrUpdateCommand>())).Returns(commandResult);

            ValidationResultWithFieldsErrorsViewModel<int?> result = Controller.EditInboundVessel(model.Object, 10);

            Assert.IsInstanceOfType(result, typeof(ValidationResultWithFieldsErrorsViewModel<int?>));
            Assert.AreEqual(result.Data, 10);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/delete", x => x.Delete(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/available-actions", x => x.GetAvailableActions(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/validate-selected-records", x => x.ValidateSelectedRecords(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/save-inbound", x => x.AddNewVessel(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/vessel/save-inbound/10", x => x.EditInboundVessel(null, 10));
        }
    }
}
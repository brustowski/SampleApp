using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class InboundRecordsControllerTests : ApiControllerFunctionTestsBase<InboundRecordsController>
    {
        private Mock<IRailInboundReadModelRepository> _railInboundReadModelRepositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IRailImportRecordsFilingValidator> _selectedInboundRecordValidatorMock;
        private Mock<ISingleRecordValidator<RailInboundReadModel>> _singleInboundRecordValidatorMock;
        private Mock<IBdParsedRepository> _bdParsedRepository;

        protected override InboundRecordsController TestInitialize()
        {
            _railInboundReadModelRepositoryMock = new Mock<IRailInboundReadModelRepository>();
            _bdParsedRepository = new Mock<IBdParsedRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _selectedInboundRecordValidatorMock = new Mock<IRailImportRecordsFilingValidator>();
            _singleInboundRecordValidatorMock = new Mock<ISingleRecordValidator<RailInboundReadModel>>();

            return new InboundRecordsController(_railInboundReadModelRepositoryMock.Object, _bdParsedRepository.Object, _searchRequestFactoryMock.Object,
                _commandDispatcherMock.Object, _pageConfigContainerMock.Object, _selectedInboundRecordValidatorMock.Object,
                _singleInboundRecordValidatorMock.Object);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _railInboundReadModelRepositoryMock.Setup(x => x.GetTotalMatchesAsync<RailInboundReadModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _railInboundReadModelRepositoryMock.Setup(x => x.GetAllOrderedAsync<RailInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<RailInboundReadModel>() { CurrentPage = 1, Results = new List<RailInboundReadModel>() });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new RailBdParsedActionsConfig());

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public async Task Search_CallsValidationForEachRecord_WhenCalled()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord1 = new RailInboundReadModel { Importer = "I", Supplier = "S", HTS = "000111" };
            var inboundRecord2 = new RailInboundReadModel { Importer = "I", Supplier = "S", HTS = "000111" };
            _searchRequestFactoryMock.Setup(x => x.Create<RailInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _railInboundReadModelRepositoryMock.Setup(x => x.GetAllOrderedAsync<RailInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<RailInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<RailInboundReadModel>
                        {
                            inboundRecord1,
                            inboundRecord2
                        }
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new RailBdParsedActionsConfig());

            await Controller.Search(searchRequestModel);
            _singleInboundRecordValidatorMock.Verify(x => x.GetErrors(It.IsAny<RailInboundReadModel>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Search_ReturnsEmptyErrorListForItemAndHighlightType_WhenItemHasNoErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new RailInboundReadModel { Importer = "I", Supplier = "S", HTS = "000111" };
            _searchRequestFactoryMock.Setup(x => x.Create<RailInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);
            _railInboundReadModelRepositoryMock.Setup(x => x.GetAllOrderedAsync<RailInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<RailInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<RailInboundReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new RailBdParsedActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<RailInboundReadModel>())).Returns(new List<string>());

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(0, result.Results.First().Errors.Count);
            Assert.AreEqual(HighlightingType.NoHighlighting, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public async Task Search_ReturnsErrorListForItemAndErrorHighlighting_WhenItemHasErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new RailInboundReadModel
            {
                BdParsedDescription1 = "descr1"
            };
            _searchRequestFactoryMock.Setup(x => x.Create<RailInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _railInboundReadModelRepositoryMock.Setup(x => x.GetAllOrderedAsync<RailInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<RailInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<RailInboundReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new RailBdParsedActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<RailInboundReadModel>())).Returns(new List<string> { "error 1", "error 2" });

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(2, result.Results.First().Errors.Count);
            Assert.AreEqual("error 1", result.Results.First().Errors.ElementAt(0));
            Assert.AreEqual("error 2", result.Results.First().Errors.ElementAt(1));
            Assert.AreEqual(HighlightingType.Error, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public void Delete_WhenCalled_DispatchDeleteCommand()
        {
            var recordIds = new int[] { 423 };

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<ICommand>())).Returns(CommandResult.Ok);

            Controller.Delete(recordIds);

            _commandDispatcherMock.Verify(x =>
                x.Dispatch(It.Is<RailInboundMassDeleteCommand>(c => c.RecordIds == recordIds)));
        }

        [TestMethod]
        public void Restore_WhenCalled_DispatchRestoreCommand()
        {
            var recordIds = new[] { 423 };

            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<ICommand>())).Returns(CommandResult.Ok);

            Controller.Restore(recordIds);

            _commandDispatcherMock.Verify(x =>
                x.Dispatch(It.Is<RailInboundMassRestoreCommand>(c => c.RecordIds == recordIds)));
        }

        [TestMethod]
        public void ValidateSelectedRecords_ReturnsOk_IfIdsIsValid()
        {
            var ids = new List<int>() { 3, 4 };
            var records = new List<RailInboundReadModel> { new RailInboundReadModel() };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(records);
            _selectedInboundRecordValidatorMock.Setup(x => x.Validate(records)).Returns(new InboundRecordValidationResult { CommonError = "" });
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new InboundRecordListActionsConfig());

            var result = Controller.ValidateSelectedRecords(ids);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void ValidateSelectedRecords_ReturnsBadRequest_IfIdsIsNotValid()
        {
            var ids = new List<int> { 3, 4 };
            var errorMessage = "error";
            var records = new List<RailInboundReadModel> { new RailInboundReadModel() };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(records);
            _selectedInboundRecordValidatorMock.Setup(x => x.Validate(records)).Returns(new InboundRecordValidationResult { CommonError = errorMessage });
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new InboundRecordListActionsConfig());

            var result = Controller.ValidateSelectedRecords(ids);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ValidateSelectedRecords_CallsPageConfiguration_ToAddAvailableActions()
        {
            var ids = new List<int> { 3, 4 };
            var records = new List<RailInboundReadModel> { new RailInboundReadModel() };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(records);
            _selectedInboundRecordValidatorMock.Setup(x => x.Validate(records)).Returns(new InboundRecordValidationResult { CommonError = "" });
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new InboundRecordListActionsConfig());

            var result = Controller.ValidateSelectedRecords(ids);

            _pageConfigContainerMock.Verify(x => x.GetPageConfig("InboundRecordListActionsConfig"), Times.Once);
            Assert.IsNotNull(result.Actions);
        }

        [TestMethod]
        public void ValidateFilteredRecords_CallsValidate_ToValidateFilteredRecords()
        {
            var filtersSet = new FiltersSet();
            _selectedInboundRecordValidatorMock.Setup(x => x.Validate(filtersSet)).Returns(new InboundRecordValidationResult { CommonError = "" });
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new InboundRecordListActionsConfig());

            var result = Controller.ValidateFilteredRecords(filtersSet);

            _selectedInboundRecordValidatorMock.Verify(x => x.Validate(filtersSet), Times.Once);

            Assert.IsNotNull(result.Actions);
        }

        [TestMethod]
        public void ValidateFilteredRecords_CallsPageConfiguration_ToAddAvailableActions()
        {
            var filtersSet = new FiltersSet();
            _selectedInboundRecordValidatorMock.Setup(x => x.Validate(filtersSet)).Returns(new InboundRecordValidationResult { CommonError = "" });
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new InboundRecordListActionsConfig());

            var result = Controller.ValidateFilteredRecords(filtersSet);

            _pageConfigContainerMock.Verify(x => x.GetPageConfig("FilteredRailRecordsActionsConfig"), Times.Once);

            Assert.IsNotNull(result.Actions);
        }

        [TestMethod]
        public void AddOrEditRecord_requires_RailFileInboundRecord_permissions()
        {
            AssertPermissions(Permissions.RailFileInboundRecord, x => x.AddOrEditRecord(null));
        }


        [TestMethod]
        public void AddNew_calls_Repository()
        {
            // Assign
            var model = new Mock<RailInboundEditModel>();

            // Act
            Controller.AddOrEditRecord(model.Object);

            // Assert
            _commandDispatcherMock.Verify(x => x.Dispatch(It.IsAny<RailImportAddOrUpdateCommand>()));
        }

        [TestMethod]
        public void GetEditModel_requires_GetManifest_Permissions()
        {
            AssertPermissions(Permissions.RailViewManifest, x => x.GetEditModel(1));
        }
    }
}
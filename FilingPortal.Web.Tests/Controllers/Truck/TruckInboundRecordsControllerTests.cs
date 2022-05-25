using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckInboundRecordsControllerTests : ApiControllerFunctionTestsBase<TruckInboundRecordsController>
    {
        private Mock<ITruckInboundReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IListInboundValidator<TruckInboundReadModel>> _selectedInboundRecordValidatorMock;
        private Mock<ISingleRecordValidator<TruckInboundReadModel>> _singleInboundRecordValidatorMock;

        protected override TruckInboundRecordsController TestInitialize()
        {
            _repositoryMock = new Mock<ITruckInboundReadModelRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _selectedInboundRecordValidatorMock = new Mock<IListInboundValidator<TruckInboundReadModel>>();
            _singleInboundRecordValidatorMock = new Mock<ISingleRecordValidator<TruckInboundReadModel>>();
            return new TruckInboundRecordsController(_repositoryMock.Object, _searchRequestFactoryMock.Object, _commandDispatcherMock.Object,
                _pageConfigContainerMock.Object, _selectedInboundRecordValidatorMock.Object, _singleInboundRecordValidatorMock.Object);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<TruckInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<TruckInboundReadModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<TruckInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckInboundReadModel>() { CurrentPage = 1, Results = new List<TruckInboundReadModel>() });

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public async Task Search_CallsValidationForEachRecord_WhenCalled()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord1 = new TruckInboundReadModel { Importer = "I", PAPs = "P" };
            var inboundRecord2 = new TruckInboundReadModel { Importer = "I", PAPs = "P" };
            _searchRequestFactoryMock.Setup(x => x.Create<TruckInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckInboundReadModel>
                        {
                            inboundRecord1,
                            inboundRecord2
                        }
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckInboundActionsConfig());

            await Controller.Search(searchRequestModel);
            _singleInboundRecordValidatorMock.Verify(x => x.GetErrors(It.IsAny<TruckInboundReadModel>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Search_ReturnsEmptyErrorListForItemAndHighlightType_WhenItemHasNoErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new TruckInboundReadModel { Importer = "I", PAPs = "P" };
            _searchRequestFactoryMock.Setup(x => x.Create<TruckInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);
            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckInboundReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckInboundActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<TruckInboundReadModel>())).Returns(new List<string>());

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(0, result.Results.First().Errors.Count);
            Assert.AreEqual(HighlightingType.NoHighlighting, result.Results.First().HighlightingType);
        }

        [TestMethod]
        public async Task Search_ReturnsErrorListForItemAndErrorHighlighting_WhenItemHasErrors()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            var inboundRecord = new TruckInboundReadModel();

            _searchRequestFactoryMock.Setup(x => x.Create<TruckInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<TruckInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<TruckInboundReadModel>
                    {
                        CurrentPage = 1,
                        Results = new List<TruckInboundReadModel>
                    {inboundRecord}
                    });

            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>()))
                .Returns(new TruckInboundActionsConfig());
            _singleInboundRecordValidatorMock.Setup(x => x.GetErrors(It.IsAny<TruckInboundReadModel>())).Returns(new List<string> { "error 1" });

            var result = await Controller.Search(searchRequestModel);

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
                x.Dispatch(It.Is<TruckInboundMassDeleteCommand>(c => c.RecordIds == recordIds)));
        }

        [TestMethod]
        public void GetAvailableActions_ReturnsRecordsAvailableActions()
        {
            var ids = new List<int>() { 3, 4 };
            var records = new List<TruckInboundReadModel> { new TruckInboundReadModel() };
            _repositoryMock.Setup(x => x.GetList(ids)).Returns(records);
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new TruckInboundListActionsConfig());

            var result = Controller.GetAvailableActions(ids);

            Assert.IsInstanceOfType(result, typeof(Actions));
        }
    }
}

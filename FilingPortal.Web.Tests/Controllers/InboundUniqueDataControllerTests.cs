using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class InboundUniqueDataControllerTests : ApiControllerFunctionTestsBase<InboundUniqueDataController>
    {
        private Mock<IRailFilingDataRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IGridConfigRegistry> _gridConfigRegistry;

        protected override InboundUniqueDataController TestInitialize()
        {
            _repositoryMock = new Mock<IRailFilingDataRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _gridConfigRegistry = new Mock<IGridConfigRegistry>();
            var gridConfig = new Mock<IGridConfiguration>();

            _gridConfigRegistry.Setup(x => x.GetGridConfig(It.IsAny<string>())).Returns(gridConfig.Object);

            return new InboundUniqueDataController(_repositoryMock.Object, _searchRequestFactoryMock.Object);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<RailFilingData>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task GetTotalMatches_AddsFilterByFilingHeaderId()
        {
            var filingHeaderId = 567;
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel
            {
                FilterSettings = new FiltersSet
                {
                    Filters = new List<Filter>
                    {
                        new Filter
                        {
                            FieldName = "FilingHeaderId",

                            Operand = FilterOperands.Equal,
                            Values = new List<LookupItem> {new LookupItem {Value = filingHeaderId}}
                        }
                    }
                }
            };
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<RailFilingData>(searchRequest))
                .ReturnsAsync(totalMatched);

            await Controller.GetTotalMatches(searchRequestModel);

            _searchRequestFactoryMock.Verify(x => x.Create<RailFilingData>(
                It.Is<SearchRequestModel>(m =>
                    m.FilterSettings.Filters.Any(f =>
                        f.Operand == FilterOperands.Equal &&
                        f.FieldName == "FilingHeaderId" &&
                        f.Values.Count == 1 && (int)
                        f.Values.First().Value == filingHeaderId))));
        }

        [TestMethod]
        public async Task GetTotalMatches_ForEmptyFilterSet_AddsFilterByFilingHeaderId()
        {
            var filingHeaderId = 567;
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel
            {
                FilterSettings = new FiltersSet
                {
                    Filters = new List<Filter>
                    {
                        new Filter
                        {
                            FieldName = "FilingHeaderId",

                            Operand = FilterOperands.Equal,
                            Values = new List<LookupItem> {new LookupItem {Value = filingHeaderId}}
                        }
                    }
                }
            };
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<RailFilingData>(searchRequest))
                .ReturnsAsync(totalMatched);

            await Controller.GetTotalMatches(searchRequestModel);

            _searchRequestFactoryMock.Verify(x => x.Create<RailFilingData>(
                It.Is<SearchRequestModel>(m =>
                    m.FilterSettings.Filters.Any(f =>
                        f.Operand == FilterOperands.Equal &&
                        f.FieldName == "FilingHeaderId" &&
                        f.Values.Count == 1 && (int)
                        f.Values.First().Value == filingHeaderId))));
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var filingHeaderId = 567;
            var searchRequestModel = new SearchRequestModel
            {
                FilterSettings = new FiltersSet
                {
                    Filters = new List<Filter>
                    {
                        new Filter
                        {
                            FieldName = "FilingHeaderId",

                            Operand = FilterOperands.Equal,
                            Values = new List<LookupItem> {new LookupItem {Value = filingHeaderId}}
                        }
                    }
                }
            };
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllWithSummaryAsync(searchRequest))
                .ReturnsAsync(
                    new PagedResultWithSummaryRow<RailFilingData>() { CurrentPage = 1, Results = new List<RailFilingData>() });

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public async Task Search_AddsFilterByFilingHeaderId()
        {
            var filingHeaderId = 567;
            var searchRequestModel = new SearchRequestModel
            {
                FilterSettings = new FiltersSet
                {
                    Filters = new List<Filter>
                    {
                        new Filter
                        {
                            FieldName = "FilingHeaderId",

                            Operand = FilterOperands.Equal,
                            Values = new List<LookupItem> {new LookupItem {Value = filingHeaderId}}
                        }
                    }
                }
            };
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllWithSummaryAsync(searchRequest))
                .ReturnsAsync(
                    new PagedResultWithSummaryRow<RailFilingData>() { CurrentPage = 1, Results = new List<RailFilingData>() });

            await Controller.Search(searchRequestModel);

            _searchRequestFactoryMock.Verify(x => x.Create<RailFilingData>(
                It.Is<SearchRequestModel>(m =>
                    m.FilterSettings.Filters.Any(f =>
                        f.Operand == FilterOperands.Equal &&
                        f.FieldName == "FilingHeaderId" &&
                        f.Values.Count == 1 && (int)
                        f.Values.First().Value == filingHeaderId))));
        }

        [TestMethod]
        public async Task Search_ForEmptyFilterSet_AddsFilterByFilingHeaderId()
        {
            var filingHeaderId = 567;
            var searchRequestModel = new SearchRequestModel
            {
                FilterSettings = new FiltersSet
                {
                    Filters = new List<Filter>
                    {
                        new Filter
                        {
                            FieldName = "FilingHeaderId",

                            Operand = FilterOperands.Equal,
                            Values = new List<LookupItem> {new LookupItem {Value = filingHeaderId}}
                        }
                    }
                }
            };
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<RailFilingData>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllWithSummaryAsync(searchRequest))
                .ReturnsAsync(
                    new PagedResultWithSummaryRow<RailFilingData>() { CurrentPage = 1, Results = new List<RailFilingData>() });

            await Controller.Search(searchRequestModel);

            _searchRequestFactoryMock.Verify(x => x.Create<RailFilingData>(
                It.Is<SearchRequestModel>(m =>
                    m.FilterSettings.Filters.Any(f =>
                        f.Operand == FilterOperands.Equal &&
                        f.FieldName == "FilingHeaderId" &&
                        f.Values.Count == 1 && (int)
                        f.Values.First().Value == filingHeaderId))));
        }
    }
}
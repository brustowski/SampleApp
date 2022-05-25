using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class LookupDataControllerTests : ApiControllerFunctionTestsBase<LookupDataController>
    {
        private Mock<IGridConfigRegistry> _gridConfigRegistryMock;
        private Mock<ILookupDataProviderRegistry> _lookupDataProviderRegistryMock;
        private Mock<IHandbookDataProvider> _handbookDataProvider;

        protected override LookupDataController TestInitialize()
        {
            _gridConfigRegistryMock = new Mock<IGridConfigRegistry>();
            _lookupDataProviderRegistryMock = new Mock<ILookupDataProviderRegistry>();
            _handbookDataProvider = new Mock<IHandbookDataProvider>();

            return new LookupDataController(_lookupDataProviderRegistryMock.Object, _gridConfigRegistryMock.Object, _handbookDataProvider.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Get, "/api/lookup/grid-filter-data?gridName=any&fieldName=any&search=&limit=20&dependValue="
                , x => x.GetFilterData(null, null, null, 20, null));
            AssertRoute(HttpMethod.Get, "/api/lookup/grid-column-data?gridName=any&fieldName=any&search=&limit=20&searchByKey=false&dependValue="
                , x => x.GetGridColumnData(null, null, null, 20, false, null));
            AssertRoute(HttpMethod.Get, "/api/lookup/data?providerName=any&search=&limit=20&searchByKey=False", x => x.GetData("any", null, 20, false, null, null));

        }

        [TestMethod]
        public void GetFilterData_ReturnsDataFromCorrectDataProvider()
        {
            var gridName = "gridName123";
            var fieldName = "fieldName1234";
            var search = "searchString";
            var limit = 200;
            var dependValue = "dependValue1";
            var gridConfigMock = new Mock<IGridConfiguration>();
            var lookupDataProvider = new Mock<ILookupDataProvider>();

            Type dataSourceType = typeof(FakeItem);
            var filterConfig = new FilterConfig(fieldName) { DataSourceType = dataSourceType };

            gridConfigMock.Setup(x => x.GetFilterConfig(fieldName)).Returns(filterConfig);

            _gridConfigRegistryMock.Setup(x => x.GetGridConfig(gridName)).Returns(gridConfigMock.Object);

            _lookupDataProviderRegistryMock.Setup(x => x.GetProvider(dataSourceType))
                .Returns(lookupDataProvider.Object);

            var filterItems = new List<LookupItem>();
            lookupDataProvider.Setup(x => x.GetData(It.Is<SearchInfo>(c => c.Search == search && c.Limit == limit)))
                .Returns(filterItems);

            IEnumerable<LookupItem> filterData = Controller.GetFilterData(gridName, fieldName, search, limit, dependValue);

            Assert.AreEqual(filterItems, filterData);
        }


        [TestMethod]
        public void GetGridColumnData_ReturnsDataFromCorrectDataProvider()
        {
            var gridName = "gridName123";
            var fieldName = "fieldName1234";
            var search = "searchString";
            var limit = 200;
            var dependValue = "dependValue1";

            System.Type dataSourceType = typeof(FakeItem);
            var columnConfig = new ColumnConfig(fieldName) { DataSourceType = dataSourceType };

            var gridConfigMock = new Mock<IGridConfiguration>();
            gridConfigMock.Setup(x => x.GetColumnConfig(fieldName)).Returns(columnConfig);
            _gridConfigRegistryMock.Setup(x => x.GetGridConfig(gridName)).Returns(gridConfigMock.Object);

            var lookupDataProvider = new Mock<ILookupDataProvider>();
            _lookupDataProviderRegistryMock.Setup(x => x.GetProvider(dataSourceType))
                .Returns(lookupDataProvider.Object);

            var items = new List<LookupItem>();
            lookupDataProvider.Setup(x => x.GetData(It.Is<SearchInfo>(c => c.Search == search && c.Limit == limit)))
                .Returns(items);

            IEnumerable<LookupItem> columnData = Controller.GetGridColumnData(gridName, fieldName, search, limit, false, dependValue);

            Assert.AreEqual(items, columnData);
        }

        [TestMethod]
        public void GetData_ReturnsDataFromCorrectDataProvider()
        {
            const string providerName = "test_provider";
            const string search = "searchString";
            const int limit = 200;

            var lookupDataProvider = new Mock<ILookupDataProvider>();
            _lookupDataProviderRegistryMock.Setup(x => x.GetProvider(providerName))
                .Returns(lookupDataProvider.Object);

            var items = new List<LookupItem>
            {
                new LookupItem{Value = "value", DisplayValue = "display value"},
                new LookupItem{Value = "value2", DisplayValue = "display value 2"}
            };

            lookupDataProvider.Setup(x => x.GetData(It.Is<SearchInfo>(c => c.Search == search && c.Limit == limit)))
                .Returns(items);

            IEnumerable<LookupItem> result = Controller.GetData(providerName, search, limit, false);

            CollectionAssert.AreEqual(items, result.ToList());
        }
    }
}
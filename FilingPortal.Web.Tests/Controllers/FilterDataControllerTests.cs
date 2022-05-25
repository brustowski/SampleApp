using System.Collections.Generic;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class FilterDataControllerTests : ApiControllerFunctionTestsBase<FilterDataController>
    {
        private Mock<IGridConfigRegistry> _gridConfigRegistryMock;
        private Mock<IFilterDataProviderRegistry> _filterDataProviderRegistryMock;
        protected override FilterDataController TestInitialize()
        {
            _gridConfigRegistryMock = new Mock<IGridConfigRegistry>();
            _filterDataProviderRegistryMock = new Mock<IFilterDataProviderRegistry>();

            return new FilterDataController(_filterDataProviderRegistryMock.Object, _gridConfigRegistryMock.Object);
        }

        [TestMethod]
        public void GetFilterData_ReturnsConfigColumnsFromCorrectGridConfiguration()
        {
            var gridName = "gridName123";
            var fieldName = "fieldName1234";
            var search = "searchString";
            var limit = 200;
            var dependOn = "dependOn1";
            var dependValue = "dependValue1";
            var gridConfigMock = new Mock<IGridConfiguration>();
            var filterDataProvider = new Mock<IFilterDataProvider>();

            var dataSourceType = typeof(FakeItem);
            var filterConfig = new FilterConfig(fieldName) { DataSourceType = dataSourceType };

            gridConfigMock.Setup(x => x.GetFilterConfig(fieldName)).Returns(filterConfig);

            _gridConfigRegistryMock.Setup(x => x.GetGridConfig(gridName)).Returns(gridConfigMock.Object);

            _filterDataProviderRegistryMock.Setup(x => x.GetProvider(dataSourceType))
                .Returns(filterDataProvider.Object);

            var filterItems = new List<LookupItem>();
            filterDataProvider.Setup(x => x.GetData(It.Is<SearchInfo>(c => c.Search == search && c.Limit == limit)))
                .Returns(filterItems);

            var filterData = Controller.GetFilterData(gridName, fieldName, search, limit, dependOn, dependValue);

            Assert.AreEqual(filterItems, filterData);
        }


        [TestMethod]
        public void GetFilterData_ReturnsFromDataProvider()
        {
            var dataProvider = "DataProvider";
            var search = "searchString";
            var limit = 200;

            var filterDataProvider = new Mock<IFilterDataProvider>();

            _filterDataProviderRegistryMock.Setup(x => x.GetProvider(dataProvider))
                .Returns(filterDataProvider.Object);

            var filterItems = new List<LookupItem>();
            filterDataProvider.Setup(x => x.GetData(It.Is<SearchInfo>(c => c.Search == search && c.Limit == limit)))
                .Returns(filterItems);

            var filterData = Controller.GetFilterData(dataProvider, search, limit);

            Assert.AreEqual(filterItems, filterData);
        }
    }
}
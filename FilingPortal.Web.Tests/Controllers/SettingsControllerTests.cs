using System.Collections.Generic;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class SettingsControllerTests : ApiControllerFunctionTestsBase<SettingsController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IGridConfigRegistry> _gridConfigRegistryMock;
        protected override SettingsController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _gridConfigRegistryMock = new Mock<IGridConfigRegistry>();

            return new SettingsController(_pageConfigContainerMock.Object, _gridConfigRegistryMock.Object);
        }

        [TestMethod]
        public void GetGridDefinition_ReturnsConfigColumnsFromCorrectGridConfiguration()
        {
            var gridName = "gridName123";
            var gridConfigMock = new Mock<IGridConfiguration>();
            var columnConfigs = new List<ColumnConfig>();
            gridConfigMock.Setup(x => x.GetColumns()).Returns(columnConfigs);

            _gridConfigRegistryMock.Setup(x => x.GetGridConfig(gridName)).Returns(gridConfigMock.Object);

            var gridDefinition = Controller.GetGridDefinition(gridName);

            _gridConfigRegistryMock.Verify(x => x.GetGridConfig(gridName));
            Assert.AreEqual(columnConfigs, gridDefinition);
        }

        [TestMethod]
        public void GetFiltersConfig_ReturnsFiltersColumnsFromCorrectGridConfiguration()
        {
            var gridName = "gridName122";
            var gridConfigMock = new Mock<IGridConfiguration>();
            var filterConfigs = new List<FilterConfig>();
            gridConfigMock.Setup(x => x.GetFilters()).Returns(filterConfigs);

            _gridConfigRegistryMock.Setup(x => x.GetGridConfig(gridName)).Returns(gridConfigMock.Object);

            var gridDefinition = Controller.GetFiltersConfig(gridName);

            _gridConfigRegistryMock.Verify(x => x.GetGridConfig(gridName));
            Assert.AreEqual(filterConfigs, gridDefinition);
        }
    }
}
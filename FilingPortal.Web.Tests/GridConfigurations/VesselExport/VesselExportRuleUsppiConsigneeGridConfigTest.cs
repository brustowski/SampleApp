using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.VesselExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.VesselExport
{
    [TestClass]
    public class VesselExportRuleUsppiConsigneeGridConfigTest
    {
        private VesselExportRuleUsppiConsigneeGridConfig _gridConfig;
        private Mock<IKeyFieldsService> _keyFieldsService;

        [TestInitialize]
        public void TestInitialize()
        {
            _keyFieldsService = new Mock<IKeyFieldsService>();
            _gridConfig = new VesselExportRuleUsppiConsigneeGridConfig(_keyFieldsService.Object);
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.VesselExportRuleUsppiConsignee, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            System.Collections.Generic.IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(7, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            System.Collections.Generic.IEnumerable<FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(5, result.Count());
        }
    }
}

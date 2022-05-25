using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.VesselExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.Web.Tests.GridConfigurations.VesselExport
{
    [TestClass]
    public class VesselExportDefaultValuesGridConfigTest
    {
        private VesselExportDefaultValuesGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new VesselExportDefaultValuesGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.VesselExportDefaultValues, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            System.Collections.Generic.IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(17, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            System.Collections.Generic.IEnumerable<FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(13, result.Count());
        }
    }
}

using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.VesselExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.Web.Tests.GridConfigurations.VesselExport
{
    [TestClass]
    public class VesselExportGridConfigTest
    {
        private VesselExportGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new VesselExportGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.VesselExportRecords, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            System.Collections.Generic.IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(27, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            System.Collections.Generic.IEnumerable<FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(31, result.Count());
        }
    }
}

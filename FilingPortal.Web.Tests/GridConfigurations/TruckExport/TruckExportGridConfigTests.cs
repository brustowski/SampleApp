using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.TruckExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.TruckExport
{
    [TestClass]
    public class TruckExportGridConfigTests
    {
        private TruckExportGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new TruckExportGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.TruckExportRecords, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(21, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(26, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectTextFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(14, result.Count(f => f.Type == "text"));
        }
    }
}

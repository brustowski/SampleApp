using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.TruckExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.TruckExport
{
    [TestClass]
    public class TruckExportDefaultValuesGridConfigTest
    {
        private TruckExportDefaultValuesGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new TruckExportDefaultValuesGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_Returns_CorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.TruckExportDefaultValues, result);
        }

        [TestMethod]
        public void GetColumns_Returns_ValidColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(17, result.Count());
        }

        [TestMethod]
        public void GetColumns_Returns_OnlyResizableColumns()
        {
            var result = _gridConfig.GetColumns().ToList();
            Assert.IsTrue(result.All(x => x.IsResizable));
        }

        [TestMethod]
        public void GetFilters_Returns_ValidFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(13, result.Count());
        }
    }
}

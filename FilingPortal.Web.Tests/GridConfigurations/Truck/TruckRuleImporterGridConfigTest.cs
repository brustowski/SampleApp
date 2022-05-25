using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.Truck
{
    [TestClass]
    public class TruckRuleImporterGridConfigTest
    {
        private TruckRuleImporterGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new TruckRuleImporterGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.TruckRuleImporter, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(29, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(7, result.Count(x => x.EditType == "text"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableFloatingPointNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(5, result.Count(x => x.EditType == "float"));
        }

        [TestMethod]
        public void GetColumns_ReturnsOnlyResizableColumns()
        {
            var result = _gridConfig.GetColumns().ToList();
            Assert.IsTrue(result.All(x => x.IsResizable));
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(27, result.Count());
        }
    }
}

using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.Truck
{
    [TestClass]
    public class TruckInboundGridConfigTests
    {
        private TruckInboundGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new TruckInboundGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.TruckInboundRecords, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(9, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectTextFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(4, result.Count(f => f.Type == "text"));
        }
    }
}

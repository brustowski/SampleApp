using FilingPortal.Web.GridConfigurations;
using FilingPortal.Web.GridConfigurations.ClientManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.GridConfigurations
{
    [TestClass]
    public class ClientsGridConfigTests
    {
        private ClientsGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new ClientsGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.Clients, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            System.Collections.Generic.IEnumerable<Web.Common.Grids.Columns.ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            System.Collections.Generic.IEnumerable<Web.Common.Grids.Filters.FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(4, result.Count());
        }
    }
}

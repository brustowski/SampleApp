using FilingPortal.Web.Common.Grids.Columns;
using FilingPortal.Web.GridConfigurations;
using FilingPortal.Web.GridConfigurations.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.GridConfigurations.Pipeline
{
    [TestClass]
    public class PipelineRuleBatchCodeGridConfigTests
    {
        private PipelineRuleBatchCodeGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new PipelineRuleBatchCodeGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.PipelineRuleBatchCode, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            System.Collections.Generic.IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            System.Collections.Generic.IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(2, result.Count(x => x.EditType == ColumnEditTypes.Text));
        }

        [TestMethod]
        public void GetColumns_Returns_OnlyResizableColumns()
        {
            var result = _gridConfig.GetColumns().ToList();
            Assert.IsTrue(result.All(x => x.IsResizable));
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            System.Collections.Generic.IEnumerable<Web.Common.Grids.Filters.FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectTextFiltersCount()
        {
            System.Collections.Generic.IEnumerable<Web.Common.Grids.Filters.FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(2, result.Where(f => f.Type == "text").Count());
        }
    }
}

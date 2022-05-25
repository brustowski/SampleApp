using FilingPortal.Web.Common.Grids.Columns;
using FilingPortal.Web.GridConfigurations.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using GridNames = FilingPortal.Web.GridConfigurations.GridNames;

namespace FilingPortal.Web.Tests.GridConfigurations.Pipeline
{
    [TestClass]
    public class PipelineRuleImporterGridConfigTests
    {
        private PipelineRuleImporterGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new PipelineRuleImporterGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.PipelineRuleImporter, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(16, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(5, result.Count(x => x.EditType == ColumnEditTypes.Text));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableFloatingPointNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(2, result.Count(x => x.EditType == ColumnEditTypes.FloatNumber));
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
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(16, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectTextFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(13, result.Count(f => f.Type == "text"));
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFloatNumberFiltersConfigs()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(2, result.Count(f => f.Type == "floatNumber"));
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectNumberFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(1, result.Count(f => f.Type == "number"));
        }
    }
}

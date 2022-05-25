using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.Web.GridConfigurations.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.Rules
{
    [TestClass]
    public class RailDefaultValuesGridConfigTest
    {
        private RailDefaultValuesGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new RailDefaultValuesGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            string result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.RailDefaultValues, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(17, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(3, result.Count(x => x.EditType == "text"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableLookupField()
        {
            IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.AreEqual(7, result.Count(x => x.EditType == "lookup"));
        }

        [TestMethod]
        public void GetColumns_ReturnsDefValueColumn()
        {
            IEnumerable<ColumnConfig> result = _gridConfig.GetColumns();

            Assert.IsTrue(result.Any(x => x.FieldName == "DefaultValue"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(3, result.Count(x => x.EditType == "integer"));
            Assert.IsTrue(result.First(x => x.FieldName == "DisplayOnUI").EditType == "integer");
            Assert.IsTrue(result.First(x => x.FieldName == "Manual").EditType == "integer");
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableBooleanField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(3, result.Count(x => x.EditType == "boolean"));
            Assert.IsTrue(result.First(x => x.FieldName == "Editable").EditType == "boolean");
            Assert.IsTrue(result.First(x => x.FieldName == "Mandatory").EditType == "boolean");
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
            IEnumerable<FilterConfig> result = _gridConfig.GetFilters();

            Assert.AreEqual(13, result.Count());
        }
    }
}

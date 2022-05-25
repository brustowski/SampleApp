using System.Linq;
using FilingPortal.Web.GridConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations
{
    [TestClass]
    public class DefaultValuesGridConfigTests
    {
        private DefaultValuesGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new DefaultValuesGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.DefaultValues, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(13, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(5, result.Count(x => x.EditType == "text"));
        }

        [TestMethod]
        public void GetColumns_ReturnsDefValueColumn()
        {
            var result = _gridConfig.GetColumns();

            Assert.IsTrue(result.Any(x => x.FieldName == "DefValue"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(2, result.Count(x => x.EditType == "integer"));
            Assert.IsTrue(result.First(x => x.FieldName == "DisplayOnUI").EditType == "integer");
            Assert.IsTrue(result.First(x => x.FieldName == "Manual").EditType == "integer");
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableBooleanField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(3, result.Count(x => x.EditType == "boolean"));
            Assert.IsTrue(result.First(x => x.FieldName == "HasDefaultValue").EditType == "boolean");
            Assert.IsTrue(result.First(x => x.FieldName == "Editable").EditType == "boolean");
            Assert.IsTrue(result.First(x => x.FieldName == "Mandatory").EditType == "boolean");
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(10, result.Count());
        }
    }
}

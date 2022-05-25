using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Services;
using FilingPortal.Web.GridConfigurations.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.Rules
{
    [TestClass]
    public class RailRuleImporterSupplierGridConfigTest
    {
        private RailRuleImporterSupplierGridConfig _gridConfig;
        private Mock<IKeyFieldsService> _keyFieldsService;

        [TestInitialize]
        public void TestInitialize()
        {
            _keyFieldsService = new Mock<IKeyFieldsService>();
            _gridConfig = new RailRuleImporterSupplierGridConfig(_keyFieldsService.Object);
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.RailRuleImporterSupplier, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(26, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(3, result.Count(x => x.EditType == "text"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(0, result.Count(x => x.EditType == "integer"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableFloatingPointNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(1, result.Count(x => x.EditType == "float"));
            Assert.IsTrue(result.First(x => x.FieldName == "Value").EditType == "float");
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

            Assert.AreEqual(22, result.Count());
        }
    }
}

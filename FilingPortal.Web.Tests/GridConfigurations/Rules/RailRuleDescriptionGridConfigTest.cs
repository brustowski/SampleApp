using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Services;
using FilingPortal.Web.GridConfigurations.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.Rules
{
    [TestClass]
    public class RailRuleDescriptionGridConfigTest
    {
        private RailRuleDescriptionGridConfig _gridConfig;
        private Mock<IKeyFieldsService> _keyFieldsService;

        [TestInitialize]
        public void TestInitialize()
        {
            _keyFieldsService = new Mock<IKeyFieldsService>();
            _gridConfig = new RailRuleDescriptionGridConfig(_keyFieldsService.Object);
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.RailRuleDescription, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(15, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(5, result.Count(x => x.EditType == "text"));
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableFloatingPointNumberField()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual(2, result.Count(x => x.EditType == "float"));
            Assert.IsTrue(result.First(x => x.FieldName == "TemplateHTSQuantity").EditType == "float");
            Assert.IsTrue(result.First(x => x.FieldName == "TemplateInvoiceQuantity").EditType == "float");
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

            Assert.AreEqual(13, result.Count());
        }
    }
}

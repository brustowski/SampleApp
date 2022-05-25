using FilingPortal.Domain.Common;
using FilingPortal.Web.GridConfigurations.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.Domain.Services;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.Vessel
{
    [TestClass]
    public class VesselRuleImporterGridConfigurationTest
    {
        private VesselRuleImporterGridConfiguration _gridConfig;
        private Mock<IKeyFieldsService> _keyFieldsService;

        [TestInitialize]
        public void TestInitialize()
        {
            _keyFieldsService = new Mock<IKeyFieldsService>();
            _gridConfig = new VesselRuleImporterGridConfiguration(_keyFieldsService.Object);
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.VesselRuleImporter, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsColumnsWithEditableTextField()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(2, result.Count(x => x.EditType == "text"));
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

            Assert.AreEqual(2, result.Count());
        }
    }
}

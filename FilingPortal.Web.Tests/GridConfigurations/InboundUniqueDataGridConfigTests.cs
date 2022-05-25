using System.Linq;
using FilingPortal.Web.GridConfigurations.Rail;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations
{
    [TestClass]
    public class InboundUniqueDataGridConfigTests
    {
        private InboundUniqueDataGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new InboundUniqueDataGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual("inbound_records_unique_data", result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(7, result.Count());
        }

        [TestMethod]
        public void GetColumns_ReturnsNotSortableColumns()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(7, result.Count(x => !x.IsSortable));
        }

        [TestMethod]
        public void GetColumns_ReturnsImporterColumnWithWidth()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual("Importer", result.ElementAt(0).DisplayName);
            Assert.AreEqual(200, result.ElementAt(0).MinWidth);
        }

        [TestMethod]
        public void GetColumns_ReturnsPortCodeColumnWithWidth()
        {
            var result = _gridConfig.GetColumns().ToList();

            Assert.AreEqual("Port Code", result.ElementAt(4).DisplayName);
            Assert.AreEqual(100, result.ElementAt(4).MaxWidth);
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsFilterByFilingHeadersId()
        {
            var result = _gridConfig.GetFilters();

            Assert.IsTrue(result.Any(x => x.FieldName == "FilingHeaderId"));
            Assert.AreEqual(FilterOperands.Equal, result.First(x => x.FieldName == "FilingHeaderId").Operand);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers
{
    [TestClass]
    public class TruckExportTableColumnsDataProviderTest
    {
        private Mock<ITablesRepository<TruckExportTable>> _repository;
        private TruckExportTableColumnsDataProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new Mock<ITablesRepository<TruckExportTable>>();
            _provider = new TruckExportTableColumnsDataProvider(_repository.Object);
        }

        [TestMethod]
        public void Name_Return_CorrectProviderName()
        {
            string result = _provider.Name;
            Assert.AreEqual("TruckExportTableColumns", result);
        }

        [TestMethod]
        public void GetData_Returns_LookupItemCollection()
        {
            _repository.Setup(x => x.GetByTableName(
                    It.IsAny<string>()))
                .Returns(new EnumerableQuery<TruckExportTable>(new[]
                {
                    new TruckExportTable() {ColumnName = "column 1"}, new TruckExportTable() {ColumnName = "column 2"}
                }));
            var searchInfo = new SearchInfo(string.Empty, 0);
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<LookupItem>));
        }

        [TestMethod]
        public void GetData_Returns_ValidLookupItem()
        {
            _repository.Setup(x => x.GetAllAsQueryable())
                .Returns(new EnumerableQuery<TruckExportTable>(new[]
                {
                    new TruckExportTable() {ColumnName = "column 1"}, new TruckExportTable() {ColumnName = "column 2"}
                }));
            var searchInfo = new SearchInfo(string.Empty, 0);
            LookupItem result = _provider.GetData(searchInfo).First();
            Assert.AreEqual("column 1", result.DisplayValue);
            Assert.AreEqual("column 1", result.Value);
        }

        [TestMethod]
        public void GetData_ReturnsEmptyCollection_WhenDependentValueIsEmpty()
        {
            var searchInfo = new SearchInfo(string.Empty, 0)
            {
                DependOn = "table_name"
            };
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetData_CallsRepository_WhenCalled()
        {
            var searchInfo = new SearchInfo("searched text", 10)
            {
                DependOn = "table_name",
                DependValue = "table name"
            }
            ;
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);

            _repository.Verify(x => x.GetByTableName(
                    It.Is<string>(v => v.Equals("table name")))
            , Times.Once);
        }

        [TestMethod]
        public void GetData_DoesNotCallsRepository_WhenDependentValueIsEmpty()
        {
            var searchInfo = new SearchInfo("searched text", 10)
                {
                    DependOn = "table_name"
                }
                ;
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);

            _repository.Verify(x => x.GetByTableName(
                    It.IsAny<string>())
                , Times.Never);
        }
    }
}

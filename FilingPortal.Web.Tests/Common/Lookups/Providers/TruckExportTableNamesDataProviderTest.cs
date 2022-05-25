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
    public class TruckExportTableNamesDataProviderTest
    {
        private Mock<ITablesRepository<TruckExportTable>> _repository;
        private TruckExportTableNamesDataProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new Mock<ITablesRepository<TruckExportTable>>();
            _provider = new TruckExportTableNamesDataProvider(_repository.Object);
        }

        [TestMethod]
        public void Name_Return_CorrectProviderName()
        {
            string result = _provider.Name;
            Assert.AreEqual("TruckExportTableNames", result);
        }

        [TestMethod]
        public void GetData_Returns_LookupItemCollection()
        {
            _repository.Setup(x => x.GetTableNames())
                .Returns(new EnumerableQuery<string>(new[] { "table 1", "table 2" }));
            var searchInfo = new SearchInfo(string.Empty, 0);
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<LookupItem>));
        }

        [TestMethod]
        public void GetData_CallsRepository_WhenCalled()
        {
            var searchInfo = new SearchInfo("searched text", 10);
            IEnumerable<LookupItem> result = _provider.GetData(searchInfo);

            _repository.Verify(
                x => x.GetTableNames(),
                Times.Once);
        }

        [TestMethod]
        public void GetData_Returns_ValidLookupItem()
        {
            _repository.Setup(x => x.GetTableNames())
                .Returns(new EnumerableQuery<string>(new[] { "table 1", "table 2" }));
            var searchInfo = new SearchInfo(string.Empty, 0);
            LookupItem result = _provider.GetData(searchInfo).First();
            Assert.AreEqual("table 1", result.DisplayValue);
            Assert.AreEqual("table 1", result.Value);
        }
    }
}

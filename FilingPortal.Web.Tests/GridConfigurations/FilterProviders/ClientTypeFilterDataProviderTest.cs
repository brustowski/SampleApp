using System.Linq;
using FilingPortal.Domain.Enums;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.FilterProviders
{
    [TestClass]
    public class ClientTypeFilterDataProviderTest
    {
        private ClientTypeFilterDataProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new ClientTypeFilterDataProvider();
        }

        [TestMethod]
        public void GetData_Returns3Items()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Count() == 3);
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithAllTitleAndClientTypeNoneValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi=> fi.DisplayValue == "All" && (ClientType)fi.Value == ClientType.None));
        }

        [TestMethod]
        public void FilterItems_ContainsFilter_WithImporterTitleAndClientTypeImporterValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == ClientType.Importer.GetDescription() && (ClientType)fi.Value == ClientType.Importer));
        }

        [TestMethod]
        public void FilterItems_ContainsFilter_WithSupplierTitleAndClientTypeSupplierValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == ClientType.Supplier.GetDescription() && (ClientType)fi.Value == ClientType.Supplier));
        }
    }
}

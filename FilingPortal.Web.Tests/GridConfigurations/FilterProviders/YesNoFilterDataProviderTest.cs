using System;
using System.Linq;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.FilterProviders
{
    [TestClass]
    public class ClientStatusDataProviderTest
    {
        private ClientStatusFilterDataProvider provider;

        [TestInitialize]
        public void TestInitialize()
        {
            provider = new ClientStatusFilterDataProvider();
        }

        [TestMethod]
        public void GetData_Returns3Items()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = provider.GetData(searchInfo);
            Assert.IsTrue(value.Count() == 3);
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithAllTitleAndNullValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi=> fi.DisplayValue == "All" && fi.Value == null));
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithActiveTitleAndTrueValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == "Active" && Convert.ToBoolean(fi.Value)));
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithDeactivatedTitleAndFalseValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == "Deactivated" && !Convert.ToBoolean(fi.Value)));
        }
    }
}

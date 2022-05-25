using System;
using System.Linq;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.FilterProviders
{
    [TestClass]
    public class YesNoFilterDataProviderTest
    {
        private YesNoFilterDataProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new YesNoFilterDataProvider();
        }

        [TestMethod]
        public void GetData_Returns3Items()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Count() == 3);
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithAllTitleAndNullValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi=> fi.DisplayValue == "All" && fi.Value == null));
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithYesTitleAndTrueValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == "Yes" && Convert.ToBoolean(fi.Value)));
        }

        [TestMethod]
        public void FilterItems_ContainsItem_WithNoTitleAndFalseValue()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var value = _provider.GetData(searchInfo);
            Assert.IsTrue(value.Any(fi => fi.DisplayValue == "No" && !Convert.ToBoolean(fi.Value)));
        }
    }
}

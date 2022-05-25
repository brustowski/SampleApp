using System.Net.Http;
using FilingPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class FilterDataControllerRoutesTests : ApiControllerTestsBase<FilterDataController>
    {
        [TestMethod]
        public void GetFilterData_RouteShouldBeExist()
        {
            const string route = "/api/filters/getfilterdata?gridName=any&fieldName=any&search=&limit=20&dependOn=&dependValue=";
            AssertRoute(HttpMethod.Get, route, x => x.GetFilterData(null, null, null, 20, null, null));
        }

        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/filters/getfilterdata?dataProviderName=a&search=b&limit=2";
            AssertRoute(HttpMethod.Get, route, x => x.GetFilterData("a", "b", 2));
        }
    }
}
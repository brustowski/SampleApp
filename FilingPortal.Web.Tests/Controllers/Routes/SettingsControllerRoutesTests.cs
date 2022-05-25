using System.Net.Http;
using FilingPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class SettingsControllerRoutesTests : ApiControllerTestsBase<SettingsController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/settings/getgridconfig?gridName=any";
            AssertRoute(HttpMethod.Get, route, x => x.GetGridDefinition(null));
        }

        [TestMethod]
        public void GetFiltersConfig_RouteShouldBeExist()
        {
            const string route = "/api/settings/getfiltersconfig?gridName=any";
            AssertRoute(HttpMethod.Get, route, x => x.GetFiltersConfig(null));
        }
    }
}
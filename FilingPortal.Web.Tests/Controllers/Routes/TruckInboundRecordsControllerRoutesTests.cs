using System.Net.Http;
using FilingPortal.Web.Controllers.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class TruckInboundRecordsControllerRoutesTests : ApiControllerTestsBase<TruckInboundRecordsController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/inbound/truck/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }
        
        [TestMethod]
        public void Search_RouteShouldBeExist()
        {
            const string route = "/api/inbound/truck/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }

        [TestMethod]
        public void Delete_RouteShouldBeExist()
        {
            const string route = "/api/inbound/truck/delete";
            AssertRoute(HttpMethod.Post, route, x => x.Delete(null));
        }

        [TestMethod]
        public void ValidateSelectedRecords_RouteShouldBeExist()
        {
            const string route = "/api/inbound/truck/available-actions";
            AssertRoute(HttpMethod.Post, route, x => x.GetAvailableActions(null));
        }
    }
}
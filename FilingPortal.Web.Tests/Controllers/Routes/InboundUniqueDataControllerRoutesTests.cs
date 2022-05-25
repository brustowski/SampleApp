using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class InboundUniqueDataControllerRoutesTests : ApiControllerTestsBase<InboundUniqueDataController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/unique-data/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }

        [TestMethod]
        public void Search_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/unique-data/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }
    }
}
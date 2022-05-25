using System.Net.Http;
using FilingPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class ClientsControllerRoutesTests : ApiControllerTestsBase<ClientsController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/clients/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }

        [TestMethod]
        public void Search_RouteShouldBeExist()
        {
            const string route = "/api/clients/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }
    }
}
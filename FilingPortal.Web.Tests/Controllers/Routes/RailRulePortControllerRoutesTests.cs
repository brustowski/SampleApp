using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class RailRulePortControllerRoutesTests : ApiControllerTestsBase<RailRulePortController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }

        [TestMethod]
        public void Search_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }

        [TestMethod]
        public void Delete_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/delete/354";
            AssertRoute(HttpMethod.Post, route, x => x.Delete(354));
        }

        [TestMethod]
        public void Add_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/create";
            AssertRoute(HttpMethod.Post, route, x => x.Create(null));
        }

        [TestMethod]
        public void GetNewRow_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/getNew";
            AssertRoute(HttpMethod.Get, route, x => x.GetNewRow());
        }

        [TestMethod]
        public void Update_RouteShouldExist()
        {
            const string route = "/api/rules/rail/port/update";
            AssertRoute(HttpMethod.Post, route, x => x.Update(null));
        }
    }
}
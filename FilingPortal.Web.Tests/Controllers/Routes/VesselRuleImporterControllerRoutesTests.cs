using System.Net.Http;
using FilingPortal.Web.Controllers.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class VesselRuleImporterControllerRoutesTests : ApiControllerTestsBase<VesselRuleImporterController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldExist()
        {
            const string route = "/api/rules/vessel/importer/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }

        [TestMethod]
        public void Search_RouteShouldBeExist()
        {
            const string route = "/api/rules/vessel/importer/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }

        [TestMethod]
        public void Update_RouteShouldBeExist()
        {
            const string route = "/api/rules/vessel/importer/update";
            AssertRoute(HttpMethod.Post, route, x => x.Update(null));
        }

        [TestMethod]
        public void Add_RouteShouldBeExist()
        {
            const string route = "/api/rules/vessel/importer/create";
            AssertRoute(HttpMethod.Post, route, x => x.Create(null));
        }

        [TestMethod]
        public void GetNewRow_RouteShouldBeExist()
        {
            const string route = "/api/rules/vessel/importer/getNew";
            AssertRoute(HttpMethod.Get, route, x => x.GetNewRow());
        }

        [TestMethod]
        public void Delete_RouteShouldBeExist()
        {
            const string route = "/api/rules/vessel/importer/delete/2";
            AssertRoute(HttpMethod.Post, route, x => x.Delete(2));
        }
    }
}
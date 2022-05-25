using System.Net.Http;
using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class PipelineInboundUniqueDataControllerTests : ApiControllerTestsBase<PipelineInboundUniqueDataController>
    {
        [TestMethod]
        public void _RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/unique-data/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }

        [TestMethod]
        public void Search_RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/unique-data/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }
    }
}
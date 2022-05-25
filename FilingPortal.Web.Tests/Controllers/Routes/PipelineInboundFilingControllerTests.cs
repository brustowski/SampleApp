using System.Net.Http;
using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class PipelineInboundFilingControllerTests : ApiControllerTestsBase<PipelineInboundFilingController>
    {
        [TestMethod]
        public void Start_RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/filing/start";
            AssertRoute(HttpMethod.Post, route, x => x.Start(null));
        }

        [TestMethod]
        public void ValidateFilingHeaderId_RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/filing/validate/12";
            AssertRoute(HttpMethod.Get, route, x => x.ValidateFilingHeaderId(12));
        }

        [TestMethod]
        public void ValidateFilingHeaderIds_RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/filing/validate";
            AssertRoute(HttpMethod.Post, route, x => x.ValidateFilingHeaderIds(new[] { 12 }));
        }

        [TestMethod]
        public void CancelFilingProcess_RouteShouldExist()
        {
            const string route = "/api/inbound/pipeline/filing/cancel";
            AssertRoute(HttpMethod.Post, route, x => x.CancelFilingProcess(new[] { 12 }));
        }

        [TestMethod]
        public void RecordIds_RouteShouldBeExist()
        {
            const string route = "/api/inbound/pipeline/filing/record-ids";
            AssertRoute(HttpMethod.Post, route, x => x.GetInboundRecordIdsByFilingHeaders(null));
        }
    }
}
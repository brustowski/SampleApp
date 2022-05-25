using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class PipelineInboundImportControllerRoutesTests: ApiControllerTestsBase<PipelineInboundImportController>
    {
        [TestMethod]
        public void ProcessFile_RouteShouldBeExist()
        {
            const string route = "/api/inbound/pipeline/upload";
            AssertRoute(HttpMethod.Post, route, x => x.ProcessFile(null));
        }
    }
}

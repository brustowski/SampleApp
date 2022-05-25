using System.Net.Http;
using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class PipelineDownloadControllerRoutesTests : ApiControllerTestsBase<PipelineDownloadDocumentsController>
    {
        [TestMethod]
        public void Download_RouteShouldBeExist()
        {
            const string route = "/api/inbound/pipeline/documents/12";
            AssertRoute(HttpMethod.Get, route, x => x.DownloadDocument(12));
        }
    }
}
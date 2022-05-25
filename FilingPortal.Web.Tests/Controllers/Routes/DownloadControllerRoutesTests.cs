using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class DownloadControllerRoutesTests : ApiControllerTestsBase<DownloadDocumentsController>
    {
        [TestMethod]
        public void EdiMessageText_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/documents/manifest/23";
            AssertRoute(HttpMethod.Get, route, x => x.DownloadManifest(23));
        }

        [TestMethod]
        public void Download_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/documents/12";
            AssertRoute(HttpMethod.Get, route, x => x.DownloadDocument(12));
        }
    }
}
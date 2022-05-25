using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class ManifestControllerRoutesTests : ApiControllerTestsBase<ManifestController>
    {
        [TestMethod]
        public void GetRecordManifest_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/manifest/2";
            AssertRoute(HttpMethod.Get, route, x => x.GetRecordManifest(2));
        }
    }
}
using System.Net.Http;
using FilingPortal.Web.Controllers.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class TruckInboundImportControllerRoutesTests : ApiControllerTestsBase<TruckInboundImportController>
    {
        [TestMethod]
        public void ProcessFile_RouteShouldBeExist()
        {
            const string route = "/api/inbound/truck/upload";
            AssertRoute(HttpMethod.Post, route, x => x.ProcessFile(null));
        }
    }
}
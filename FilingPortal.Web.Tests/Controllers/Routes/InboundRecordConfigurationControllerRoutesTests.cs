using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class InboundRecordConfigurationControllerRoutesTests : ApiControllerTestsBase<InboundRecordConfigurationController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/field-config/34";
            AssertRoute(HttpMethod.Post, route, x => x.GetConfiguration(34));
        }
    }
}
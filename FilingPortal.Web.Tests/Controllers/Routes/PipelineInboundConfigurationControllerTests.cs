using System.Net.Http;
using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class PipelineInboundConfigurationControllerTests : ApiControllerTestsBase<PipelineInboundConfigurationController>
    {
        [TestMethod]
        public void GetConfiguration_RouteShouldBeExist()
        {
            const string route = "/api/inbound/pipeline/field-config/12";
            AssertRoute(HttpMethod.Post, route, x => x.GetConfiguration(12));
        }
    }
}
using System.Net.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class RailFormConfigControllerTests: ApiControllerFunctionTestsBase<RailFormConfigController>
    {
        private Mock<IFormConfigFactory<RailInboundEditModel>> _factory;

        protected override RailFormConfigController TestInitialize()
        {
            _factory = new Mock<IFormConfigFactory<RailInboundEditModel>>();

            return new RailFormConfigController(_factory.Object);
        }

        [TestMethod]
        public void GetAddFormConfig_requires_RailFileInboundRecord_permissions()
        {
            AssertPermissions(Permissions.RailFileInboundRecord, x => x.GetAddFormConfig());
        }

        [TestMethod]
        public void GetAddFormConfig_Has_Correct_Route()
        {
            AssertRoute(HttpMethod.Get, "/api/inbound/rail/get-add-form-config", x => x.GetAddFormConfig());
        }
    }
}
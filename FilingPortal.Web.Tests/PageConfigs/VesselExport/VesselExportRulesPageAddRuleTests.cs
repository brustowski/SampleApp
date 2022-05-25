using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportRulesPageAddRuleTests
    {
        private VesselExportRulesPageAddRule _action;

        private static PageConfigurationModel CreateModel()
        {
            var mock = new Mock<PageConfigurationModel>();
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportRulesPageAddRule();
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(CreateModel(), resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasPermissions_ReturnTrue()
        {
            var resourceRequestor = new FakeResourceRequestor(true);
            var result = _action.IsAvailable(CreateModel(), resourceRequestor);

            Assert.IsTrue(result);
        }
    }
}

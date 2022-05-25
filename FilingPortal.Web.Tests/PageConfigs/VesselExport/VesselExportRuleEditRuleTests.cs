using FilingPortal.Domain.Entities;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportRuleEditRuleTests
    {
        private VesselExportRuleEditRule _action;

        private static IRuleEntity CreateModel()
        {
            var mock = new Mock<IRuleEntity>();
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportRuleEditRule();
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

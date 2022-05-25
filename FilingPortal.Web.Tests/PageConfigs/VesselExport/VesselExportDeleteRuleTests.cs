using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportDeleteRuleTests
    {
        private FakeResourceRequestor _resourceRequestor;

        private VesselExportDeleteRule _action;

        private static VesselExportReadModel CreateModel(bool canBeDone = true)
        {
            var mock = new Mock<VesselExportReadModel>();
            mock.Setup(x => x.CanBeDeleted()).Returns(canBeDone);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportDeleteRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(CreateModel(), resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRuleCanNotBeDeleted_ReturnFalse()
        {
            var result = _action.IsAvailable(CreateModel(false), _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRuleCanBeDeleted_ReturnTrue()
        {
            var result = _action.IsAvailable(CreateModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}

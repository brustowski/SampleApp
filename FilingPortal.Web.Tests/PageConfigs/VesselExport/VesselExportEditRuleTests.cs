using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportEditRuleTests
    {
        private FakeResourceRequestor _resourceRequestor;

        private VesselExportEditRule _action;

        private static VesselExportReadModel CreateModel(bool canBeDone = true)
        {
            var mock = new Mock<VesselExportReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(canBeDone);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportEditRule();
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
        public void IsAvailable_IfRuleCanNotBeEdited_ReturnFalse()
        {
            var result = _action.IsAvailable(CreateModel(false), _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRuleCanBeEdited_ReturnTrue()
        {
            var result = _action.IsAvailable(CreateModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}

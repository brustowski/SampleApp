using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Web.PageConfigs.TruckExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.TruckExport
{
    [TestClass]
    public class TruckExportDeleteRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private TruckExportDeleteRule _action;

        private TruckExportReadModel CreateModel(bool canBeDone = true)
        {
            var mock = new Mock<TruckExportReadModel>();
            mock.Setup(x => x.CanBeDeleted()).Returns(canBeDone);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new TruckExportDeleteRule();
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
        public void IsAvailable_IfModelCanNotBeDeleted_ReturnFalse()
        {
            var result = _action.IsAvailable(CreateModel(false), _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllModelsCanBeCanceled_ReturnTrue()
        {
            var result = _action.IsAvailable(CreateModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}

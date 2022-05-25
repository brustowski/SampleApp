using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class RailBdParsedEditRuleTest
    {
        RailBdParsedEditRule _action;
        private FakeResourceRequestor _resourceRequestor;

        private RailInboundReadModel BuildModel(bool editable = true)
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(editable);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new RailBdParsedEditRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_No_Model_Returns_False()
        {
            var result = _action.IsAvailable(null, _resourceRequestor);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsAvailable_Non_Editable_Model_returns_False()
        {
            var record = BuildModel(editable: false);

            var result = _action.IsAvailable(record, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Editable_Model_returns_True()
        {
            var record = BuildModel();

            var result = _action.IsAvailable(record, _resourceRequestor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAvailable_NoPermissions_Returns_False()
        {
            var record = BuildModel();

            var resourceRequestor = new FakeResourceRequestor(false);

            var result = _action.IsAvailable(record, resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Calls_CanBeEdited()
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(true);

            var result = _action.IsAvailable(mock.Object, _resourceRequestor);

            mock.Verify(x => x.CanBeEdited(), Times.Once);
        }
    }
}
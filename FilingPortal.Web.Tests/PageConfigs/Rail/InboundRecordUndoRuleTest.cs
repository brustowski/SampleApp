using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.Rail
{
    [TestClass]
    public class InboundRecordCancelRuleTest
    {
        InboundRecordListCancelRule _action;
        private FakeResourceRequestor _resourceRequestor;

        private RailInboundReadModel BuildModel(bool cancelable = true)
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeCanceled()).Returns(cancelable);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new InboundRecordListCancelRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_No_Models_Returns_False()
        {
            var records = new List<RailInboundReadModel>(0);
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsAvailable_Non_Editable_Models_returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(cancelable: false),
                BuildModel(cancelable: false)
            };
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Editable_Models_returns_True()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(),
                BuildModel()
            };
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAvailable_AtLeastOne_Model_Is_Non_Cancelable_Returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(cancelable: false),
                BuildModel(cancelable: true)
            };
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_NoPermissions_Returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel()
            };

            var resourceRequestor = new FakeResourceRequestor(false);

            var result = _action.IsAvailable(records, resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Calls_CanBeCanceled()
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeCanceled()).Returns(true);

            var records = new List<RailInboundReadModel>()
            {
                mock.Object
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            mock.Verify(x => x.CanBeCanceled(), Times.Once);
        }
    }
}
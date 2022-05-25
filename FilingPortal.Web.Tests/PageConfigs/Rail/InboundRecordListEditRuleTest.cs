using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.Rail
{
    [TestClass]
    public class InboundRecordListEditRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private InboundRecordListEditRule _action;

        private RailInboundReadModel BuildModel(bool editable = true)
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(editable);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new InboundRecordListEditRule();
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
                BuildModel(editable: false),
                BuildModel(editable: false)
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
        public void IsAvailable_AtLeastOne_Model_Is_Non_Editable_Returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(editable: false),
                BuildModel(editable: true)
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
        public void IsAvailable_Calls_IsEditable()
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(true);

            var records = new List<RailInboundReadModel>()
            {
                mock.Object
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            mock.Verify(x => x.CanBeEdited(), Times.Once);
        }
    }
}
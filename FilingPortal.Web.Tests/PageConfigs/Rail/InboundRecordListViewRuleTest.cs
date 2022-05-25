using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.Rail
{
    [TestClass]
    public class InboundRecordListViewRuleTest
    {
        InboundRecordListViewRule _action;
        private FakeResourceRequestor _resourceRequestor;

        private RailInboundReadModel BuildModel(bool viewable = true)
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeViewed()).Returns(viewable);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new InboundRecordListViewRule();
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
        public void IsAvailable_Non_Viewable_Models_returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(viewable: false),
                BuildModel(viewable: false)
            };
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Viewable_Models_returns_True()
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
        public void IsAvailable_AtLeastOne_Model_Is_Non_Viewable_Returns_False()
        {
            var records = new List<RailInboundReadModel>()
            {
                BuildModel(viewable: false),
                BuildModel(viewable: true)
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
        public void IsAvailable_Calls_CanBeViewed()
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeViewed()).Returns(true);

            var records = new List<RailInboundReadModel>()
            {
                mock.Object
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            mock.Verify(x => x.CanBeViewed(), Times.Once);
        }

    }
}
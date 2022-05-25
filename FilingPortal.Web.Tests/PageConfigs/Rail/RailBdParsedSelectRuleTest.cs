using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.PageConfigs.Rail
{
    [TestClass]
    public class RailBdParsedSelectRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private RailBdParsedSelectRule _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _action = new RailBdParsedSelectRule();
        }

        private RailInboundReadModel BuildModel(bool selectable = true, string importer = "Importer", string supplier = "supplier", string hts = "HTS")
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeSelected()).Returns(selectable);
            var result =  mock.Object;
            result.HTS = hts;
            result.Importer = importer;
            result.Supplier = supplier;
            return result;
        }

        [TestMethod]
        public void IsAvailable_No_Model_Returns_False()
        {
            var result = _action.IsAvailable(null, _resourceRequestor);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsAvailable_Non_Selectable_Model_returns_False()
        {
            var record = BuildModel(selectable: false);

            var result = _action.IsAvailable(record, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_Selectable_Model_returns_True()
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
        public void IsAvailable_Calls_CanBeSelected()
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeSelected()).Returns(true);

            var result = _action.IsAvailable(mock.Object, _resourceRequestor);

            mock.Verify(x => x.CanBeSelected(), Times.Once);
        }
    }
}

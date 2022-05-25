using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.Vessel
{
    [TestClass]
    public class TruckInboundActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private TruckInboundActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new TruckInboundActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.TruckInboundActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new TruckInboundReadModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(3, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "Select"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
        }
    }
}

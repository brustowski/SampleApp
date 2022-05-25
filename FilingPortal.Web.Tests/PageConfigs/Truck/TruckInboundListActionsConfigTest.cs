using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.Truck
{
    [TestClass]
    public class TruckInboundListActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private TruckInboundListActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new TruckInboundListActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.TruckListInboundActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new List<TruckInboundReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "ReviewFile"));
            Assert.IsTrue(actions.Any(x => x.Key == "View"));
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
        }
    }
}

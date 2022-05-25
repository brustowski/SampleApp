using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class RailBdParsedActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private RailBdParsedActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new RailBdParsedActionsConfig();
            _config.Configure();
            
        }

        [TestMethod]
        public void PageName_is_RailBdParsedActionsConfig()
        {
            var result = _config.PageName;

            Assert.AreEqual("RailBdParsedActionsConfig", result);
        }

        [TestMethod]
        public void GetActions_CallsCanBeSelected()
        {
            var obj = new RailInboundReadModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "Select"));
        }

        [TestMethod]
        public void GetActions_CallsCanBeViewedManifest()
        {
            var obj = new RailInboundReadModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x=>x.Key == "ViewManifest"));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class InboundRecordListActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private InboundRecordListActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new InboundRecordListActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_is_RailBdParsedActionsConfig()
        {
            var result = _config.PageName;

            Assert.AreEqual("InboundRecordListActionsConfig", result);
        }

        [TestMethod]
        public void GetActions_CallsCanCancel()
        {
            var obj = new List<RailInboundReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "Undo"));
        }

        [TestMethod]
        public void GetActions_CallsCanReviewAndFile()
        {
            var obj = new List<RailInboundReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "ReviewFile"));
        }

        [TestMethod]
        public void GetActions_CallsCanBeView()
        {
            var obj = new List<RailInboundReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "View"));
        }

        [TestMethod]
        public void GetActions_CallsCanSingleFile()
        {
            var obj = new List<RailInboundReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "SingleFiling"));
        }
    }
}
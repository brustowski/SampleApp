using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs.Vessel
{
    [TestClass]
    public class VesselRuleActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselRuleActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselRuleActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselRuleConfigName, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new VesselRuleImporter();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
            Assert.IsTrue(actions.Any(x => x.Key == "Copy"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
        }

        [TestMethod]
        public void GetActions_IfRequestorHasPermissions_ReturnActionsEqualToTrue()
        {
            var obj = new VesselRuleImporter();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.All(x => x.Value == true));
        }

        [TestMethod]
        public void GetActions_IfRequestorHasNoPermissions_ReturnActionsEqualToFalse()
        {
            var obj = new VesselRuleImporter();
            var resourceRequestor = new FakeResourceRequestor(false);
            var actions = _config.GetActions(obj, resourceRequestor).ToList();

            Assert.IsTrue(actions.All(x => x.Value == false));
        }
    }
}

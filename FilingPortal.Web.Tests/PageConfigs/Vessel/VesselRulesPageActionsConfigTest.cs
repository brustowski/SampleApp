using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.Vessel
{
    [TestClass]
    public class VesselRulesPageActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselRulesPageActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselRulesPageActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselRulesPageActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new PageConfigurationModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(3, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
        }

        [TestMethod]
        public void GetActions_IfRequestorHasPermissions_ReturnActionsEqualToTrue()
        {
            var obj = new PageConfigurationModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.All(x => x.Value == true));
        }

        [TestMethod]
        public void GetActions_IfRequestorHasNoPermissions_ReturnActionsEqualToFalse()
        {
            var obj = new PageConfigurationModel();
            var resourceRequestor = new FakeResourceRequestor(false);
            var actions = _config.GetActions(obj, resourceRequestor).ToList();

            Assert.IsTrue(actions.All(x => x.Value == false));
        }
    }
}

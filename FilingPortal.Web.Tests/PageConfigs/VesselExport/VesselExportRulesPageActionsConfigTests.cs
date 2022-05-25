using System.Linq;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportRulesPageActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselExportRulesPageActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselExportRulesPageActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselExportRulesPageActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new PageConfigurationModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(3, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
        }
    }
}

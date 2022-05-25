using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Tests.PageConfigs.Configurations
{
    [TestClass]
    public class ConfigurationPageActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private ConfigurationPageActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new ConfigurationPageActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.ConfigurationPageActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new PageConfigurationModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(1, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
        }
    }
}

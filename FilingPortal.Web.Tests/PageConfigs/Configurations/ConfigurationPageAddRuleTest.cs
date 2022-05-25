using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs.Configurations
{
    [TestClass]
    public class ConfigurationPageAddRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private ConfigurationPageAddRule _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new ConfigurationPageAddRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(new PageConfigurationModel(), resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllModelsCanBeCanceled_ReturnTrue()
        {
            var result = _action.IsAvailable(new PageConfigurationModel(), _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}

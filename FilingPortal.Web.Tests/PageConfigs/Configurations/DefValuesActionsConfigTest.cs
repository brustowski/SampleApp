using System.Linq;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs.Configurations
{
    [TestClass]
    public class DefValuesActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private DefValuesActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new DefValuesActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.DefValueActionsConfigName, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new DefValuesViewModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
            Assert.IsTrue(actions.Any(x => x.Key == "Copy"));
        }
    }
}

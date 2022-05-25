using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using FilingPortal.Domain.Entities;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportRuleActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselExportRuleActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselExportRuleActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselExportRuleConfigName, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            IRuleEntity obj = new Mock<IRuleEntity>().Object;
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Add"));
            Assert.IsTrue(actions.Any(x => x.Key == "Copy"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
        }
    }
}

using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselExportActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselExportActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselExportActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new VesselExportReadModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "Select"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
            Assert.IsTrue(actions.Any(x => x.Key == "EditInitial"));
        }
    }
}

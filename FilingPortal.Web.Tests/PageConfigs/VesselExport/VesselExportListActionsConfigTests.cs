using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportListActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private VesselExportListActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new VesselExportListActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.VesselExportListActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new List<VesselExportReadModel>();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(4, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "Undo"));
            Assert.IsTrue(actions.Any(x => x.Key == "ReviewFile"));
            Assert.IsTrue(actions.Any(x => x.Key == "View"));
        }
    }
}

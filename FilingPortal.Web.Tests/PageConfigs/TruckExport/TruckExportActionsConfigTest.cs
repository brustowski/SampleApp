using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.PageConfigs.TruckExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Web.Tests.PageConfigs.TruckExport
{
    [TestClass]
    public class TruckExportActionsConfigTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private TruckExportActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new TruckExportActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_Returns_CorrectPageName()
        {
            var result = _config.PageName;

            Assert.AreEqual(PageConfigNames.TruckExportActions, result);
        }

        [TestMethod]
        public void GetActions_ReturnAllAvailableActions()
        {
            var obj = new TruckExportReadModel();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.AreEqual(3, actions.Count);
            Assert.IsTrue(actions.Any(x => x.Key == "Delete"));
            Assert.IsTrue(actions.Any(x => x.Key == "Select"));
            Assert.IsTrue(actions.Any(x => x.Key == "Edit"));
        }
    }
}

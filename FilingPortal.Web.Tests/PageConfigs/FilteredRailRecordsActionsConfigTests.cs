using System.Linq;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class FilteredRailRecordsActionsConfigTests
    {
        private FakeResourceRequestor _resourceRequestor;
        private FilteredRailRecordsActionsConfig _config;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _config = new FilteredRailRecordsActionsConfig();
            _config.Configure();
        }

        [TestMethod]
        public void PageName_is_Valid()
        {
            var result = _config.PageName;

            Assert.AreEqual("FilteredRailRecordsActionsConfig", result);
        }

        [TestMethod]
        public void GetActions_ContainsSelectAll()
        {
            var obj = new InboundRecordValidationResult();
            var actions = _config.GetActions(obj, _resourceRequestor).ToList();

            Assert.IsTrue(actions.Any(x => x.Key == "SelectAll"));
        }
    }
}
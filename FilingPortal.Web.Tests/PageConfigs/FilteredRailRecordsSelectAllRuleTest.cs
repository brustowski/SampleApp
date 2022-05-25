using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class FilteredRailRecordsSelectAllRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;

        private FilteredRailRecordsSelectAllRule _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new FilteredRailRecordsSelectAllRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_ReturnsTrue_IfResultIsValid()
        {
            var obj = new InboundRecordValidationResult();

            var result = _action.IsAvailable(obj, _resourceRequestor);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAvailable_ReturnsFalse_IfresultContainsError()
        {
            var obj = new InboundRecordValidationResult
            {
                CommonError = "some error text"
            };

            var result = _action.IsAvailable(obj, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_ReturnsFalse_IfRequestorHasNoPermissions()
        {
            var obj = new InboundRecordValidationResult();

            var requestor = new FakeResourceRequestor(false);

            var result = _action.IsAvailable(obj, requestor);

            Assert.IsFalse(result);
        }
    }
}
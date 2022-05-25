using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Tests.PageConfigs
{
    [TestClass]
    public class RailBdParsedDeleteRuleTest
    {
        private FakeResourceRequestor _resourceRequestor;
        private RailListDeleteRule _action;

        [TestInitialize]
        public void TestInitialize()
        {
            _resourceRequestor = new FakeResourceRequestor();
            _action = new RailListDeleteRule();
        }

        [TestMethod]
        public void IsAvailable_ReturnTrue_IfRecordCanBeDeleted()
        {
            var obj = new RailInboundReadModel();

            Assert.AreEqual(true, _action.IsAvailable(new List<RailInboundReadModel>() { obj }, _resourceRequestor));
        }

        [TestMethod]
        public void IsAvailable_ReturnFalse_IfRecordMarkedAsDeleted()
        {
            var obj = new RailInboundReadModel { IsDeleted = true };

            Assert.AreEqual(false, _action.IsAvailable(new List<RailInboundReadModel>() { obj }, _resourceRequestor));
        }

        [TestMethod]
        public void IsAvailable_ReturnTrue_IfRecordMappingStatusHasOpen()
        {
            var obj = new RailInboundReadModel { MappingStatus = MappingStatus.Open };

            Assert.AreEqual(true, _action.IsAvailable(new List<RailInboundReadModel>() { obj }, _resourceRequestor));
        }

        [TestMethod]
        public void IsAvailable_ReturnFalse_IfRecordMappingStatusInReview()
        {
            var obj = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };

            Assert.IsTrue(_action.IsAvailable(new List<RailInboundReadModel>() { obj }, _resourceRequestor));
        }

        [TestMethod]
        public void IsAvailable_ReturnTrue_IfRecordFilingStatusHasOpen()
        {
            var obj = new RailInboundReadModel { FilingStatus = FilingStatus.Open };

            Assert.AreEqual(true, _action.IsAvailable(new List<RailInboundReadModel>() { obj }, _resourceRequestor));
        }

        [TestMethod]
        public void IsAvailable_ReturnFalse_IfRequestorHasNoPermissions()
        {
            var obj = new RailInboundReadModel();

            var requestor = new FakeResourceRequestor(false);

            var result = _action.IsAvailable(new List<RailInboundReadModel>() { obj }, requestor);

            Assert.IsFalse(result);
        }
    }
}

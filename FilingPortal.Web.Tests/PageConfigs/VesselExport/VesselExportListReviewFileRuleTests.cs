using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportListReviewFileRuleTests
    {
        private FakeResourceRequestor _resourceRequestor;

        private VesselExportListReviewFileRule _action;

        private static VesselExportReadModel CreateModel(bool canBeFiled = true)
        {
            var mock = new Mock<VesselExportReadModel>();
            mock.Setup(x => x.CanBeFiled()).Returns(canBeFiled);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportListReviewFileRule();
            _resourceRequestor = new FakeResourceRequestor();
        }

        [TestMethod]
        public void IsAvailable_WithoutData_Returns_False()
        {
            var records = new List<VesselExportReadModel>(0);
            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRequestorHasNoPermissions_ReturnFalse()
        {
            var records = new List<VesselExportReadModel>
            {
                CreateModel()
            };
            var resourceRequestor = new FakeResourceRequestor(false);
            var result = _action.IsAvailable(records, resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfRuleCanNotBeFiled_ReturnFalse()
        {
            var records = new List<VesselExportReadModel>
            {
                CreateModel(false)
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfOneOfRulesCanNotBeFiled_ReturnFalse()
        {
            var records = new List<VesselExportReadModel>
            {
                CreateModel(false),
                CreateModel()
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfAllRulesCanBeFiled_ReturnTrue()
        {
            var records = new List<VesselExportReadModel>
            {
                CreateModel(),
                CreateModel()
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsTrue(result);
        }

    }
}

using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Web.PageConfigs.VesselExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Web.Tests.PageConfigs.VesselExport
{
    [TestClass]
    public class VesselExportListEditRuleTests
    {
        private FakeResourceRequestor _resourceRequestor;

        private VesselExportListEditRule _action;

        private static VesselExportReadModel CreateModel(bool canBeEdited = true)
        {
            var mock = new Mock<VesselExportReadModel>();
            mock.Setup(x => x.CanBeEdited()).Returns(canBeEdited);
            return mock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _action = new VesselExportListEditRule();
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
        public void IsAvailable_IfRuleCanNotBeEdited_ReturnFalse()
        {
            var records = new List<VesselExportReadModel>
            {
                CreateModel(false)
            };

            var result = _action.IsAvailable(records, _resourceRequestor);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAvailable_IfOneOfRulesCanNotBeEdited_ReturnFalse()
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
        public void IsAvailable_IfAllRulesCanBeEdited_ReturnTrue()
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

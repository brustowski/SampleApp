using System;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Validators.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Validators.Rail
{
    [TestClass]
    public class RailRulePortEditModelValidatorTests
    {
        // todo: add validation for lookup dependant fields

        private RailRulePortEditModelValidator _validator;
        private Mock<ILookupMasterRepository<LookupMaster>> _lookupMasterRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _lookupMasterRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _validator = new RailRulePortEditModelValidator(_lookupMasterRepositoryMock.Object);
        }

        
        RailRulePortEditModel CreateValidModel(Action<RailRulePortEditModel> action = null)
        {
            var model = new RailRulePortEditModel
            {
                Id = 1,
                Port = "Port",
                Origin = "Origin",
                Destination = "Destination",
                FIRMsCode = "FIRMsCode",
                Export = "Export"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            var model = CreateValidModel();

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            var model = CreateValidModel(x =>
            {
                x.Origin = string.Empty;
                x.Destination = string.Empty;
                x.FIRMsCode = string.Empty;
                x.Export = string.Empty;
            });

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            var model = CreateValidModel(x =>
            {
                x.Origin = null;
                x.Destination = null;
                x.FIRMsCode = null;
                x.Export = null;
            });

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }
        
        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            var model = CreateValidModel(x => x.Id = -1);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortEmpty()
        {
            var model = CreateValidModel(x => x.Port = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Port is required", result.Errors.First(x => x.PropertyName == "Port").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortNull()
        {
            var model = CreateValidModel(x => x.Port = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Port is required", result.Errors.First(x => x.PropertyName == "Port").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPortNotExceedsLength()
        {
            var model = CreateValidModel(x => x.Port = new string('i', 128));

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }
        
        [TestMethod]
        public void Validate_ReturnsFalse_IfPortExceedsLength()
        {
            var model = CreateValidModel(x => x.Port = new string('i', 129));

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Port").ErrorMessage);
        }
        
        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableStringFieldsNotExceedsLength()
        {
            var model = CreateValidModel(x =>
            {
                x.Origin = new string('i', 128);
                x.Destination = new string('i', 128);
                x.FIRMsCode = new string('i', 128);
                x.Export = new string('i', 128);
            });

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfNullableStringFieldsExceedsLength()
        {
            var model = CreateValidModel(x =>
            {
                x.Origin = new string('i', 129);
                x.Destination = new string('i', 129);
                x.FIRMsCode = new string('i', 129);
                x.Export = new string('i', 129);
            });

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(4, result.Errors.Count);
            Assert.IsTrue(result.Errors.All(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }
    }
}

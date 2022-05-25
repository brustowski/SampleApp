using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Models.Truck;
using FilingPortal.Web.Validators.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Validators.Truck
{
    [TestClass]
    public class TruckRulePortEditModelValidatorTests
    {
        private TruckRulePortEditModelValidator _validator;
        private Mock<ILookupMasterRepository<LookupMaster>> _lookupMasterRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _lookupMasterRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _validator = new TruckRulePortEditModelValidator(_lookupMasterRepositoryMock.Object);
        }

        TruckRulePortEditModel CreateValidModel(Action<TruckRulePortEditModel> action = null)
        {
            var model = new TruckRulePortEditModel
            {
                Id = 1,
                EntryPort = "Entry Port",
                ArrivalPort = "Arrival Port",
                FIRMsCode = "FIRMsCode",
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfEntryPort_ExistinLookupRepository()
        {
            var model = CreateValidModel(x => x.EntryPort = "Entry Port");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfArrivalPort_ExistinLookupRepository()
        {
            var model = CreateValidModel(x => x.EntryPort = "Arrival Port");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfFIRMsCode_ExistinLookupRepository()
        {
            var model = CreateValidModel(x => x.EntryPort = "FIRMs Code");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
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
                x.ArrivalPort = string.Empty;
                x.FIRMsCode = string.Empty;
            });

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            var model = CreateValidModel(x =>
            {
                x.ArrivalPort = null;
                x.FIRMsCode = null;
            });

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIfArrivalPort_NotexistinLookupRepository()
        {
            var model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidPort));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfEntryPort_NotexistinLookupRepository()
        {
            var model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidPort));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFIRMsCode_NotexistinLookupRepository()
        {
            var model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidFIRMs));
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
            var model = CreateValidModel(x => x.EntryPort = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Entry Port is required", result.Errors.First(x => x.PropertyName == "EntryPort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortNull()
        {
            var model = CreateValidModel(x => x.EntryPort = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Entry Port is required", result.Errors.First(x => x.PropertyName == "EntryPort").ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues(int length, bool expectedResult)
        {
            var model = CreateValidModel(x =>
            {
                x.EntryPort = new string('i', length);
                x.ArrivalPort = new string('i', length);
                x.FIRMsCode = new string('i', length);
            });

            var result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength()
        {
            var model = CreateValidModel(x =>
            {
                x.EntryPort = new string('i', 129);
            });

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }
    }
}

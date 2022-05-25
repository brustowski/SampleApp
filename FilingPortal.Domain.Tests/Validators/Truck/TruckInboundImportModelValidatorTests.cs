using FilingPortal.Domain.Imports.Truck.Inbound;
using FilingPortal.Domain.Repositories.Clients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation.Results;

namespace FilingPortal.Domain.Tests.Validators.Truck
{
    [TestClass]
    public class TruckInboundImportModelValidatorTests
    {
        private Validator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(true);
            _clientRepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(true);

            _validator = new Validator(_clientRepositoryMock.Object);
        }

        private static ImportModel CreateValidModel(Action<ImportModel> action = null)
        {
            var model = new ImportModel
            {
                Importer = "Importer",
                Supplier = "Supplier",
                PAPs = "PAPs",
                RowNumberInFile = 1
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            ImportModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Importer = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ImporterIsRequired, result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNull()
        {
            ImportModel model = CreateValidModel(x => x.Importer = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ImporterIsRequired, result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.Importer = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporter_DoesNotExist()
        {
            const string importerName = "bad_importer";
            ImportModel model = CreateValidModel(x => x.Importer = importerName);

            _clientRepositoryMock.Setup(x => x.IsImporterValid(importerName)).Returns(false);

            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidImporter));
        }
        #endregion

        #region Supplier
        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierIsEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Supplier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.SupplierIsRequired, result.Errors.First(x => x.PropertyName == "Supplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierIsNull()
        {
            ImportModel model = CreateValidModel(x => x.Supplier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.SupplierIsRequired, result.Errors.First(x => x.PropertyName == "Supplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.Supplier = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplier_DoesNotExist()
        {
            const string value = "bad_supplier";
            ImportModel model = CreateValidModel(x => x.Supplier = value);
            _clientRepositoryMock.Setup(x => x.IsSupplierValid(value)).Returns(false);
            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidSupplier));
        }
        #endregion

        #region PAPs
        [TestMethod]
        public void Validate_ReturnsFalse_IfPAPsExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.PAPs = new string('i', 21);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 20 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfPAPsIsEmpty()
        {
            ImportModel model = CreateValidModel(x => x.PAPs = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.PAPsIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfPAPsIsNull()
        {
            ImportModel model = CreateValidModel(x => x.PAPs = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.PAPsIsRequired, result.Errors.First().ErrorMessage);
        } 
        #endregion

    }
}

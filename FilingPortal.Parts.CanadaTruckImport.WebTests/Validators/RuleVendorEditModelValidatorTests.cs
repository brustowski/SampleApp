using System;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleVendor;
using FilingPortal.Parts.CanadaTruckImport.Web.Validators;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Parts.CanadaTruckImport.WebTests.Validators
{
    [TestClass]
    public class RuleVendorEditModelValidatorTests
    {
        private RuleVendorEditModelValidator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IUnlocoDictionaryRepository> _unlocoRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new Client());
            _unlocoRepositoryMock = new Mock<IUnlocoDictionaryRepository>();
            _unlocoRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns(new UnlocoDictionaryEntry());
            _validator = new RuleVendorEditModelValidator(_clientRepositoryMock.Object, _unlocoRepositoryMock.Object);
        }

        private static RuleVendorEditModel CreateValidModel(Action<RuleVendorEditModel> action = null)
        {
            var model = new RuleVendorEditModel
            {
                Id = 1,
                VendorId = Guid.NewGuid().ToString(),
                ImporterId = Guid.NewGuid().ToString(),
                ExporterId = Guid.NewGuid().ToString(),
                ExportState = "WA",
                DirectShipPlace = "USBWS",
                NoPackages = "1",
                CountryOfOrigin = "US",
                OrgState = "WA"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RuleVendorEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.NoPackages = null;
                x.DirectShipPlace = null;
                x.OrgState = null;
                x.ExportState = null;
                x.CountryOfOrigin = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.NoPackages = string.Empty;
                x.DirectShipPlace = string.Empty;
                x.OrgState = string.Empty;
                x.CountryOfOrigin = string.Empty;
                x.ExportState = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        #region Vendor
        [TestMethod]
        public void Validate_ReturnsFalse_IfVendorPropertyEmpty()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.VendorId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Vendor"),
                result.Errors.First(x => x.PropertyName == "VendorId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfVendorPropertyNull()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.VendorId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Vendor"),
                result.Errors.First(x => x.PropertyName == "VendorId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfVendorHasWrongFormat()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.VendorId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "VendorId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfVendorIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleVendorEditModel model = CreateValidModel(x => x.VendorId = guid.ToString());

            _clientRepositoryMock.Setup(x => x.Get(guid)).Returns<Client>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Vendor", result.Errors.First(x => x.PropertyName == "VendorId").ErrorMessage);
        }
        #endregion

        #region ImporterId
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyEmpty()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ImporterId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyNull()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ImporterId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIdHasWrongFormat()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ImporterId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIdIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleVendorEditModel model = CreateValidModel(x => x.ImporterId = guid.ToString());

            _clientRepositoryMock.Setup(x => x.Get(guid)).Returns<Client>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer", result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }
        #endregion

        #region ExporterId
        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterPropertyEmpty()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ExporterId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Exporter"),
                result.Errors.First(x => x.PropertyName == "ExporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterPropertyNull()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ExporterId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Exporter"),
                result.Errors.First(x => x.PropertyName == "ExporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterIdHasWrongFormat()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.ExporterId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ExporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterIdIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleVendorEditModel model = CreateValidModel(x => x.ExporterId = guid.ToString());

            _clientRepositoryMock.Setup(x => x.Get(guid)).Returns<Client>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Exporter", result.Errors.First(x => x.PropertyName == "ExporterId").ErrorMessage);
        }
        #endregion

        #region ExportState
        [DataRow(2, true, "")]
        [DataRow(3, false, "The field must be up to 2 characters long")]
        [DataTestMethod]
        public void Validate_ExportStateBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.ExportState = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "ExportState").ErrorMessage);
            }
        }
        #endregion

        #region DirectShipPlace
        [DataRow(128, true, "")]
        [DataRow(129, false, "The field must be up to 128 characters long")]
        [DataTestMethod]
        public void Validate_DirectShipPlaceBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.DirectShipPlace = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "DirectShipPlace").ErrorMessage);
            }
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDirectShipPlaceIsNotExist()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.DirectShipPlace = "AAAAA");

            _unlocoRepositoryMock.Setup(x => x.GetByCode("AAAAA")).Returns((UnlocoDictionaryEntry)null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Direct Shipment Place", result.Errors.First(x => x.PropertyName == "DirectShipPlace").ErrorMessage);
        }
        #endregion

        #region NoPackages
        [TestMethod]
        public void Validate_ReturnsFalse_IfNoPackagesHasWrongFormat()
        {
            RuleVendorEditModel model = CreateValidModel(x => x.NoPackages = "wrong no");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IntegerFormatMismatch,
                result.Errors.First(x => x.PropertyName == "NoPackages").ErrorMessage);
        }
        #endregion

        #region CountryOfOrigin
        [DataRow(2, true, "")]
        [DataRow(3, false, "The field must be up to 2 characters long")]
        [DataTestMethod]
        public void Validate_CountryOfOriginBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.CountryOfOrigin = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "CountryOfOrigin").ErrorMessage);
            }
        }
        #endregion

        #region OrgState
        [DataRow(128, true, "")]
        [DataRow(129, false, "The field must be up to 128 characters long")]
        [DataTestMethod]
        public void Validate_OrgStateBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.OrgState = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "OrgState").ErrorMessage);
            }
        }

        [TestMethod]
        public void Validate_ReturnFalse_IfOrgStateIsEmptyAndCountryOfOriginIsUS()
        {
            RuleVendorEditModel model = CreateValidModel(x =>
            {
                x.CountryOfOrigin = "US";
                x.OrgState = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Org State"),
                result.Errors.First(x => x.PropertyName == "OrgState").ErrorMessage);
        }
        #endregion
    }
}

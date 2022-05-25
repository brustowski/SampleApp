using FilingPortal.Domain;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Validators.Rail;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Web.Tests.Validators.Rail
{
    [TestClass]
    public class RailRuleImporterSupplierEditModelValidatorTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IClientAddressRepository> _addressRepositoryMock;
        private RailRuleImporterSupplierEditModelValidator _validator;
        private Mock<ILookupMasterRepository<LookupMaster>> _portRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(true);
            _clientRepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(true);
            _addressRepositoryMock = new Mock<IClientAddressRepository>();
            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns(new ClientAddress());
            _portRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();


            _validator = new RailRuleImporterSupplierEditModelValidator(_clientRepositoryMock.Object, _addressRepositoryMock.Object, _portRepositoryMock.Object);
        }

        private RailRuleImporterSupplierEditModel CreateValidModel(Action<RailRuleImporterSupplierEditModel> action = null)
        {
            var model = new RailRuleImporterSupplierEditModel
            {
                Id = 1,
                ImporterName = "importer 1",
                SupplierName = "supplier 2",
                ValueRecon = "recon value",
                PaymentType = "4",
                MainSupplier = "main supplier",
                MainSupplierAddress = "main supplier address",
                CountryOfOrigin = "USA",
                Importer = "importer N",
                ShipToParty = "ship to party",
                Consignee = "consignee",
                BrokerToPay = "broker to pay",
                Relationship = "rel",
                DestinationState = "CA",
                Destination = "CA",
                DFT = "dft",
                FTARecon = "fta",
                FreightComposite = new RailRuleFreightComposite
                {
                    Freight = 234,
                    FreightType = 0,
                },
                Manufacturer = "manufacturer",
                ManufacturerAddress = "manufacturer address",
                Seller = "seller",
                SoldToParty = "sold to party",
                Value = "345"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x =>
            {
                x.ValueRecon = "";
                x.PaymentType = "";
                x.MainSupplierAddress = "";
                x.CountryOfOrigin = "";
                x.ShipToParty = "";
                x.Consignee = "";
                x.BrokerToPay = "";
                x.Relationship = "";
                x.Destination = "";
                x.DestinationState = "";
                x.DFT = "";
                x.FTARecon = "";
                x.Manufacturer = "";
                x.ManufacturerAddress = "";
                x.Seller = "";
                x.SoldToParty = "";
                x.Value = "";

            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x =>
            {
                x.ValueRecon = null;
                x.PaymentType = null;
                x.MainSupplierAddress = null;
                x.CountryOfOrigin = null;
                x.ShipToParty = null;
                x.Consignee = null;
                x.BrokerToPay = null;
                x.Relationship = null;
                x.Destination = null;
                x.DestinationState = null;
                x.DFT = null;
                x.FTARecon = null;
                x.Manufacturer = null;
                x.ManufacturerAddress = null;
                x.Seller = null;
                x.SoldToParty = null;
                x.Value = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterNameEmpty()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.ImporterName = "");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Importer Name is required", result.Errors.First(x => x.PropertyName == "ImporterName").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterNameNull()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.ImporterName = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Importer Name is required", result.Errors.First(x => x.PropertyName == "ImporterName").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupporterNameEmpty()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.SupplierName = "");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Supplier Name is required", result.Errors.First(x => x.PropertyName == "SupplierName").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupporterNameNull()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.SupplierName = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Supplier Name is required", result.Errors.First(x => x.PropertyName == "SupplierName").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfImporterNameNotExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.ImporterName = new string('i', 128));

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfSupplierNameNotExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.SupplierName = new string('i', 128));

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterNameExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.ImporterName = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "ImporterName").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierNameExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.SupplierName = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "SupplierName").ErrorMessage);
        }

        #region Main Supplier
        [TestMethod]
        public void Validate_ReturnsFalse_IfMainSupplierIsNull()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.MainSupplier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Main Supplier is required", result.Errors.First(x => x.PropertyName == "MainSupplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfMainSupplierExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.MainSupplier = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "MainSupplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfMainSupplierIsNotExist()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            _clientRepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidSupplier, result.Errors.First(x => x.PropertyName == "MainSupplier").ErrorMessage);
        }
        #endregion

        #region Main Supplier Address
        [TestMethod]
        public void Validate_ReturnsFalse_IfMainSupplierAddressIsNotExist()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientAddress>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidSupplierAddress, result.Errors.First(x => x.PropertyName == "MainSupplierAddress").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfMainSupplierAddressExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.MainSupplierAddress = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "MainSupplierAddress").ErrorMessage);
        }
        #endregion

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNull()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Importer = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Importer is required", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Importer = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNotExist()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            _clientRepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidImporter, result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }
        #endregion

        #region Manufacturer
        [TestMethod]
        public void Validate_ReturnsFalse_IfManufacturerExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Manufacturer = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Manufacturer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManufacturerIsNotExist()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            _clientRepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidManufacturer, result.Errors.First(x => x.PropertyName == "Manufacturer").ErrorMessage);
        }
        #endregion

        #region Manufacturer Address
        [TestMethod]
        public void Validate_ReturnsFalse_IfManufacturerAddressIsNotExist()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel();

            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientAddress>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidManufacturerAddress, result.Errors.First(x => x.PropertyName == "ManufacturerAddress").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManufacturerAddressExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.ManufacturerAddress = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "ManufacturerAddress").ErrorMessage);
        }
        #endregion

        #region Destination
        [TestMethod]
        public void Validate_ReturnsFalse_IfDestinationExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Destination = new string('i', 3));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 2 characters long", result.Errors.First(x => x.PropertyName == "Destination").ErrorMessage);
        }
        #endregion

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableStringFieldsNotExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x =>
            {
                x.ValueRecon = new string('i', 128);
                x.MainSupplier = new string('i', 128);
                x.CountryOfOrigin = new string('i', 128);
                x.Importer = new string('i', 128);
                x.ShipToParty = new string('i', 128);
                x.Consignee = new string('i', 128);
                x.BrokerToPay = new string('i', 128);
                x.Relationship = new string('i', 128);
                x.DestinationState = new string('i', 128);
                x.Destination = new string('i', 2);
                x.DFT = new string('i', 128);
                x.FTARecon = new string('i', 128);
                x.Manufacturer = new string('i', 128);
                x.Seller = new string('i', 128);
                x.SoldToParty = new string('i', 128);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfNullableStringFieldsExceedsLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x =>
            {
                x.ValueRecon = new string('i', 129);
                x.MainSupplier = new string('i', 129);
                x.CountryOfOrigin = new string('i', 129);
                x.Importer = new string('i', 129);
                x.DestinationState = new string('i', 129);
                x.ShipToParty = new string('i', 129);
                x.Consignee = new string('i', 129);
                x.BrokerToPay = new string('i', 129);
                x.Relationship = new string('i', 129);
                x.DFT = new string('i', 129);
                x.FTARecon = new string('i', 129);
                x.Manufacturer = new string('i', 129);
                x.Seller = new string('i', 129);
                x.SoldToParty = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(14, result.Errors.Count);
            Assert.IsTrue(result.Errors.All(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPaymentTypeIsCorrectInteger()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.PaymentType = "12345");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPaymentTypeIsDecimal()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.PaymentType = "12345.89");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - integer", result.Errors.First(x => x.PropertyName == "PaymentType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPaymentTypeIsLetters()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.PaymentType = "234sret34");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - integer", result.Errors.First(x => x.PropertyName == "PaymentType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPaymentTypeExceedsIntegerLength()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.PaymentType = int.MaxValue.ToString() + "1");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - integer", result.Errors.First(x => x.PropertyName == "PaymentType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfValueIsCorrectDecimalWithoutPoint()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Value = "12345");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfValueIsCorrectDecimalWithPoint()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Value = "123456.55");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfValueIsDecimalWith17NumbersBeforePoint()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Value = "123456789012.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfValueIsDecimalWithMoreThan12NumbersBeforePoint()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Value = "1234567890123.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "Value").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfValueIsDecimalWithLetters()
        {
            RailRuleImporterSupplierEditModel model = CreateValidModel(x => x.Value = "34dfsd.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "Value").ErrorMessage);
        }
    }
}

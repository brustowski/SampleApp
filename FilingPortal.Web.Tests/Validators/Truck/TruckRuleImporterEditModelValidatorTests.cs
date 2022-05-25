using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Models.Truck;
using FilingPortal.Web.Validators.Truck;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Validators.Truck
{
    [TestClass]
    public class TruckRuleImporterEditModelValidatorTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<ILookupMasterRepository<LookupMaster>> _lookupMasterRepositoryMock;
        private TruckRuleImporterEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(true);
            _clientRepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(true);
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _lookupMasterRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _validator = new TruckRuleImporterEditModelValidator(_lookupMasterRepositoryMock.Object, _clientRepositoryMock.Object);
        }

        private TruckRuleImporterEditModel CreateValidModel(Action<TruckRuleImporterEditModel> action = null)
        {
            var model = new TruckRuleImporterEditModel
            {
                Id = 1,
                CWIOR = "CWIOR",
                CWSupplier = "CWSupplier",
                ConsigneeCode = "consignee code",
                ArrivalPort = "Arrival Port",
                CE = "CE",
                Charges = 123456789012.123456M.ToString(),
                CO = "CO",
                CustomAttrib1 = "CustomAttrib1",
                CustomAttrib2 = "CustomAttrib2",
                CustomQuantity = 123456789012.123456M.ToString(),
                CustomUQ = "CustomUQ",
                DestinationState = "DestinationState",
                EntryPort = "Entry Port",
                GoodsDescription = "GoodsDescription",
                GrossWeight = 123456789012.123456M.ToString(),
                GrossWeightUQ = "GrossWeightUQ",
                InvoiceQTY = 123456789012.123456M.ToString(),
                InvoiceUQ = "InvoiceUQ",
                LinePrice = 123456789012.123456M.ToString(),
                ManufacturerMID = "ManufacturerMID",
                NAFTARecon = "NAFTARecon",
                ProductID = "ProductID",
                ReconIssue = "ReconIssue",
                SPI = "SPI",
                SupplierMID = "SupplierMID",
                Tariff = "Tariff",
                TransactionsRelated = "TransactionsRelated"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            TruckRuleImporterEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [DataRow(0, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues(int length, bool expectedResult)
        {
            TruckRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.ArrivalPort = new string('i', length);
                x.CE = new string('i', length);
                x.CO = new string('i', length);
                x.CustomAttrib1 = new string('i', length);
                x.CustomAttrib2 = new string('i', length);
                x.CustomUQ = new string('i', length);
                x.DestinationState = new string('i', length);
                x.EntryPort = new string('i', length);
                x.GoodsDescription = new string('i', length);
                x.GrossWeightUQ = new string('i', length);
                x.InvoiceUQ = new string('i', length);
                x.ManufacturerMID = new string('i', length);
                x.NAFTARecon = new string('i', length);
                x.ProductID = new string('i', length);
                x.ReconIssue = new string('i', length);
                x.SPI = new string('i', length);
                x.SupplierMID = new string('i', length);
                x.Tariff = new string('i', length);
                x.TransactionsRelated = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [DataRow("", true)]
        [DataRow("1234567890123.123456", false)]
        [DataRow("123456789012.123456", true)]
        [DataRow("1E-06", true)]
        [DataTestMethod]
        public void Validate_DecimalBoundaryValues(string value, bool expectedResult)
        {
            TruckRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.CustomQuantity = value;
                x.Charges = value;
                x.GrossWeight = value;
                x.InvoiceQTY = value;
                x.LinePrice = value;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfDecimalFormatMismatch()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CustomQuantity = "1234567890123.1234567");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.DecimalFormatMismatch));
        }

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsEmpty()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWIOR = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Importer is required", result.Errors.First(x => x.PropertyName == "CWIOR").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNull()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWIOR = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Importer is required", result.Errors.First(x => x.PropertyName == "CWIOR").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterExceedsLength()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.CWIOR = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporter_DoesNotExist()
        {
            const string importerName = "bad_importer";
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWIOR = importerName);

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
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWSupplier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Supplier is required", result.Errors.First(x => x.PropertyName == "CWSupplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierIsNull()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWSupplier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Supplier is required", result.Errors.First(x => x.PropertyName == "CWSupplier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierExceedsLength()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.CWSupplier = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplier_DoesNotExist()
        {
            const string value = "bad_supplier";
            TruckRuleImporterEditModel model = CreateValidModel(x => x.CWSupplier = value);
            _clientRepositoryMock.Setup(x => x.IsSupplierValid(value)).Returns(false);
            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidSupplier));
        }
        #endregion

        #region Consignee
        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeCodeIsEmpty()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.ConsigneeCode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeCodeIsNull()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.ConsigneeCode = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCodeExceedsLength()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.ConsigneeCode = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCode_DoesNotExist()
        {
            const string value = "bad_consignee_code";
            TruckRuleImporterEditModel model = CreateValidModel(x => x.ConsigneeCode = value);
            _clientRepositoryMock.Setup(x => x.IsExist(value)).Returns(false);
            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidConsignee));
        }
        #endregion

        [TestMethod]
        public void Validate_ReturnsTrue_IfEntryPortIsValid()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.EntryPort = "Entry Port");
            ValidationResult result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfArrivalPortIsValid()
        {
            TruckRuleImporterEditModel model = CreateValidModel(x => x.EntryPort = "Arrival Port");
            ValidationResult result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIfArrivalPort_NotexistinLookupRepository()
        {
            TruckRuleImporterEditModel model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidPort));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfEntryPort_NotexistinLookupRepository()
        {
            TruckRuleImporterEditModel model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            ValidationResult result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidPort));
        }
    }
}

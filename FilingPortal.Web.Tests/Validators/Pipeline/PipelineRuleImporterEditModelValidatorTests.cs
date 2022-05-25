using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.Validators.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Validators.Pipeline
{
    [TestClass]
    public class PipelineRuleImporterEditModelValidatorTests
    {
        private Mock<IClientRepository> _clientrepositoryMock;
        private Mock<ILookupMasterRepository<LookupMaster>> _lookupMasterRepositoryMock;
        private PipelineRuleImporterEditModelValidator _validator;


        [TestInitialize]
        public void TestInitialize()
        {
            _lookupMasterRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _clientrepositoryMock = new Mock<IClientRepository>();
            _clientrepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(true);
            _clientrepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(true);

            _validator =
                new PipelineRuleImporterEditModelValidator(_lookupMasterRepositoryMock.Object,
                    _clientrepositoryMock.Object);

        }

        private PipelineRuleImporterEditModel CreateValidModel(Action<PipelineRuleImporterEditModel> action = null)
        {
            var model = new PipelineRuleImporterEditModel
            {
                Id = 1,
                Consignee = "Consignee",
                CountryOfExport = "CountryOfExport",
                Freight = 123456789012.123456M.ToString(),
                FTARecon = "FTARecon",
                Importer = "Importer",
                Manufacturer = "Manufacturer",
                ManufacturerAddress = "ManufacturerAddress",
                MID = "MID",
                Origin = "Origin",
                Seller = "Seller",
                Supplier = "Supplier",
                TransactionRelated = "TransactionRelated",
                Value = 123456789012.123456M.ToString(),
                ReconIssue = "ReconIssue",
                SPI = "SPI"
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
        public void Validate_ReturnsTrue_IfNullableFieldsNullOrEmpty()
        {
            var model = new PipelineRuleImporterEditModel
            {
                Id = 1,
                Importer = "Importer",
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfCountryOfExport_ExistinLookupRepository()
        {
            var model = CreateValidModel(x => x.CountryOfExport = "CountryOfExport");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfImporter_ExistinClientRepository()
        {
            var model = CreateValidModel(x => x.Importer = "Importer");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfSupplier_ExistinClientRepository()
        {
            var model = CreateValidModel(x => x.Supplier = "Supplier");
            var result = _validator.Validate(model);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCountryOfExport_NotexistinLookupRepository()
        {
            var model = CreateValidModel();
            _lookupMasterRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidExport));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporter_NotexistinClientRepository()
        {
            var model = CreateValidModel();
            _clientrepositoryMock.Setup(x => x.IsImporterValid(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidImporter));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplier_NotexistinClientRepository()
        {
            var model = CreateValidModel();
            _clientrepositoryMock.Setup(x => x.IsSupplierValid(It.IsAny<string>())).Returns(false);
            var result = _validator.Validate(model);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.InvalidSupplier));
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
        public void Validate_ReturnsFalse_WithEmptyRequiredFields()
        {
            var model = CreateValidModel(x => x.Importer = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithNullRequiredFields()
        {
            var model = CreateValidModel(x => x.Importer = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues(int length, bool expectedResult)
        {
            var model = CreateValidModel(x =>
            {
                x.Consignee = new string('i', length);
                x.CountryOfExport = new string('i', length);
                x.FTARecon = new string('i', length);
                x.Importer = new string('i', length);
                x.Manufacturer = new string('i', length);
                x.ManufacturerAddress = new string('i', length);
                x.MID = new string('i', length);
                x.Origin = new string('i', length);
                x.ReconIssue = new string('i', length);
                x.Seller = new string('i', length);
                x.SPI = new string('i', length);
                x.Supplier = new string('i', length);
                x.TransactionRelated = new string('i', length);
            });

            var result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength()
        {
            var model = CreateValidModel(x => { x.Importer = new string('i', 129); });

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [DataRow("", true)]
        [DataRow("1234567890123.123456", false)]
        [DataRow("123456789012.123456", true)]
        [DataRow("1E-06", true)]
        [DataTestMethod]
        public void Validate_DecimalBoundaryValues(string value, bool expectedResult)
        {
            var model = CreateValidModel(x =>
            {
                x.Freight = value;
                x.Value = value;
            });

            var result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfDecimalFormatMismatch()
        {
            var model = CreateValidModel(x => x.Freight = "1234567890123.1234567");

            var result = _validator.Validate(model);

            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == ValidationMessages.DecimalFormatMismatch));
        }
    }
}

using System;
using System.Linq;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Import.Inbound;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Parts.Inbond.Domain.Tests.Validators
{
    [TestClass]
    public class InbondImportValidatorTests
    {
        private Validator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IFirmsCodesRepository> _firmsCodesRepositoryMock;
        private Mock<IDataProviderRepository<InBondCarrier, string>> _inBondCarrierRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _firmsCodesRepositoryMock = new Mock<IFirmsCodesRepository>();
            _firmsCodesRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _inBondCarrierRepositoryMock = new Mock<IDataProviderRepository<InBondCarrier, string>>();
            _inBondCarrierRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(new InBondCarrier());

            _validator = new Validator(_firmsCodesRepositoryMock.Object, _clientRepositoryMock.Object, _inBondCarrierRepositoryMock.Object);
        }

        private static ImportModel CreateValidModel(Action<ImportModel> action = null)
        {
            var model = new ImportModel
            {
                FirmsCode = "A001",
                ImporterCode = "ADVENGTEC",
                PortOfArrival = "2240",
                PortOfDestination = "0009",
                ExportConveyance = "ec",
                ConsigneeCode = "ADVENGTEC",
                Carrier = "BUCCARHOU",
                Value = 12M,
                ManifestQty = 21M,
                ManifestQtyUnit = "T",
                Weight = 22M,
                RowNumberInFile = 2
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            ImportModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        #region FIRMs Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.FirmsCode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "FIRMs Code"),
                result.Errors.First(x => x.PropertyName == "FirmsCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.FirmsCode = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "FIRMs Code"),
                result.Errors.First(x => x.PropertyName == "FirmsCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodeFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.FirmsCode = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "FirmsCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodeIsNotExist()
        {
            ImportModel model = CreateValidModel(x => x.FirmsCode = "A000");

            _firmsCodesRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown FIRMs Code", result.Errors.First(x => x.PropertyName == "FirmsCode").ErrorMessage);
        }
        #endregion

        #region ImporterCode Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterCodePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ImporterCode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer Code"),
                result.Errors.First(x => x.PropertyName == "ImporterCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterCodePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ImporterCode = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer Code"),
                result.Errors.First(x => x.PropertyName == "ImporterCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterCodeFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.ImporterCode = new string('i', 13);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 12 characters long", result.Errors.First(x => x.PropertyName == "ImporterCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterCodeIsNotExist()
        {
            ImportModel model = CreateValidModel(x => x.ImporterCode = "unknown");

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer Code", result.Errors.First(x => x.PropertyName == "ImporterCode").ErrorMessage);
        }
        #endregion

        #region PortOfArrival Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfArrivalPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.PortOfArrival = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port of Arrival"),
                result.Errors.First(x => x.PropertyName == "PortOfArrival").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfArrivalPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.PortOfArrival = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port of Arrival"),
                result.Errors.First(x => x.PropertyName == "PortOfArrival").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfArrivalFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.PortOfArrival = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "PortOfArrival").ErrorMessage);
        }
        #endregion

        #region PortOfDestination Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfDestinationPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.PortOfDestination = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port of Destination"),
                result.Errors.First(x => x.PropertyName == "PortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfDestinationPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.PortOfDestination = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port of Destination"),
                result.Errors.First(x => x.PropertyName == "PortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfDestinationFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.PortOfDestination = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "PortOfDestination").ErrorMessage);
        }
        #endregion

        #region ExportConveyance Code
        [TestMethod]
        public void Validate_ReturnsTrue_IfExportConveyancePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ExportConveyance = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfExportConveyancePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ExportConveyance = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportConveyanceFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.ExportConveyance = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "ExportConveyance").ErrorMessage);
        }
        #endregion

        #region ConsigneeCode Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCodePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeCode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee Code"),
                result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCodePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeCode = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee Code"),
                result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCodeFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.ConsigneeCode = new string('i', 13);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 12 characters long", result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeCodeIsNotExist()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeCode = "unknown");

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Consignee Code", result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }
        #endregion

        #region Carrier
        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Carrier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"),
                result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.Carrier = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"),
                result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.Carrier = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierIsNotExist()
        {
            ImportModel model = CreateValidModel(x => x.Carrier = "unknown");

            _inBondCarrierRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns<InBondCarrier>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown In-Bond Carrier", result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }
        #endregion

        #region Value
        [TestMethod]
        public void Validate_ReturnsFalse_IfValuePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Value = default(decimal));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Value"),
                result.Errors.First(x => x.PropertyName == "Value").ErrorMessage);
        }
        #endregion

        #region ManifestQty
        [TestMethod]
        public void Validate_ReturnsFalse_IfManifestQtyPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ManifestQty = default(decimal));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Manifest Quantity"),
                result.Errors.First(x => x.PropertyName == "ManifestQty").ErrorMessage);
        }
        #endregion

        #region ManifestQtyUnit Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfManifestQtyUnitPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ManifestQtyUnit = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Manifest Quantity Unit"),
                result.Errors.First(x => x.PropertyName == "ManifestQtyUnit").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManifestQtyUnitPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ManifestQtyUnit = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Manifest Quantity Unit"),
                result.Errors.First(x => x.PropertyName == "ManifestQtyUnit").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManifestQtyUnitFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.ManifestQtyUnit = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "ManifestQtyUnit").ErrorMessage);
        }
        #endregion

        #region Weight
        [TestMethod]
        public void Validate_ReturnsFalse_IfWeightPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Weight = default(decimal));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Weight"),
                result.Errors.First(x => x.PropertyName == "Weight").ErrorMessage);
        }
        #endregion
    }
}

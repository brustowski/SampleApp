using System;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Import.RuleEntry;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Parts.Inbond.Domain.Tests.Validators
{
    [TestClass]
    public class RuleEntryImportValidatorTests
    {
        private Validator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IFirmsCodesRepository> _firmsCodesRepositoryMock;
        private Mock<IDataProviderRepository<InBondCarrier, string>> _inBondCarrierRepositoryMock;
        private Mock<IDataProviderRepository<EntryType, string>> _entryTypeRepositoryMock;
        private Mock<IDataProviderRepository<DomesticPort, int>> _domesticPortRepositoryMock;
        private Mock<ITariffRepository<HtsTariff>> _tariffRepositoryMock;
        private Mock<ITransportModeRepository> _transportModeRepositoryMock;
        private Mock<IClientAddressRepository> _addressRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _firmsCodesRepositoryMock = new Mock<IFirmsCodesRepository>();
            _firmsCodesRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _inBondCarrierRepositoryMock = new Mock<IDataProviderRepository<InBondCarrier, string>>();
            _inBondCarrierRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(new InBondCarrier());
            _entryTypeRepositoryMock = new Mock<IDataProviderRepository<EntryType, string>>();
            _entryTypeRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(new EntryType());
            _domesticPortRepositoryMock = new Mock<IDataProviderRepository<DomesticPort, int>>();
            _domesticPortRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>())).Returns(new[] { new DomesticPort() });
            _tariffRepositoryMock = new Mock<ITariffRepository<HtsTariff>>();
            _tariffRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _transportModeRepositoryMock = new Mock<ITransportModeRepository>();
            _transportModeRepositoryMock.Setup(x => x.GetByNumber(It.IsAny<string>())).Returns(new TransportMode());
            _addressRepositoryMock = new Mock<IClientAddressRepository>();
            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns(new ClientAddress());

            _validator = new Validator(_clientRepositoryMock.Object
                , _firmsCodesRepositoryMock.Object
                , _inBondCarrierRepositoryMock.Object
                , _entryTypeRepositoryMock.Object
                , _domesticPortRepositoryMock.Object
                , _tariffRepositoryMock.Object
                , _transportModeRepositoryMock.Object
                , _addressRepositoryMock.Object);
        }

        private static ImportModel CreateValidModel(Action<ImportModel> action = null)
        {
            var model = new ImportModel
            {
                FirmsCode = "A001",
                Importer = "ADVENGTEC",
                Carrier = "TESREFSAT",
                Consignee = "ADVENGTEC",
                ConsigneeAddress = "2030 NORTH TALBOT RD",
                UsPortOfDestination = "1108",
                EntryType = "61",
                Shipper = "3DCONNORQ",
                Tariff = "123456789012",
                PortOfPresentation = "5204",
                ForeignDestination = "99999",
                TransportMode = "70"
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
        public void Validate_ReturnsFalse_IfFirmsCodeIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _firmsCodesRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown FIRMs Code", result.Errors.First(x => x.PropertyName == "FirmsCode").ErrorMessage);
        }
        #endregion

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Importer = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.Importer = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }
        #endregion

        #region Importer Address
        [TestMethod]
        public void Validate_ReturnsTrue_IfImporterAddressPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ImporterAddress = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfImporterAddressPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ImporterAddress = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterAddressIsNotExist()
        {
            var guid = Guid.NewGuid();
            ImportModel model = CreateValidModel(x => x.ImporterAddress = guid.ToString());

            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientAddress>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer Address", result.Errors.First(x => x.PropertyName == "ImporterAddress").ErrorMessage);
        }
        #endregion

        #region Carrier
        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Carrier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"), result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.Carrier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"), result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.Carrier = new string('i', 129);
            });

            _inBondCarrierRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns(new InBondCarrier());

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrier_IsNotExist()
        {
            ImportModel model = CreateValidModel();

            _inBondCarrierRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns<InBondCarrier>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown In-Bond Carrier", result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }
        #endregion

        #region Consignee
        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Consignee = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee"),
                result.Errors.First(x => x.PropertyName == "Consignee").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.Consignee = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee"),
                result.Errors.First(x => x.PropertyName == "Consignee").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeIsNotExist()
        {
            var guid = Guid.NewGuid();
            ImportModel model = CreateValidModel(x => x.Consignee = guid.ToString());

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Consignee", result.Errors.First(x => x.PropertyName == "Consignee").ErrorMessage);
        }
        #endregion

        #region Consignee Address
        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeAddressPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeAddress = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeAddressPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeAddress = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeAddressIsNotExist()
        {
            var guid = Guid.NewGuid();
            ImportModel model = CreateValidModel(x => x.ConsigneeAddress = guid.ToString());

            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientAddress>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Consignee Address", result.Errors.First(x => x.PropertyName == "ConsigneeAddress").ErrorMessage);
        }
        #endregion

        #region US Port of Destination
        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.UsPortOfDestination = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "US Port Of Destination"), result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.UsPortOfDestination = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "US Port Of Destination"),
                result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.UsPortOfDestination = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _domesticPortRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>())).Returns(Enumerable.Empty<DomesticPort>().ToList());

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown US Port Of Destination", result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }
        #endregion

        #region Entry Type
        [TestMethod]
        public void Validate_ReturnsTrue_IfEntryTypePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.EntryType = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfEntryTypePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.EntryType = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfEntryTypeFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.EntryType = new string('i', 3);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 2 characters long", result.Errors.First(x => x.PropertyName == "EntryType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfEntryTypeIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _entryTypeRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).Returns<EntryType>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Entry Type", result.Errors.First(x => x.PropertyName == "EntryType").ErrorMessage);
        }
        #endregion

        #region Tariff
        [TestMethod]
        public void Validate_ReturnsTrue_IfTariffPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.Tariff = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTariffPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.Tariff = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.Tariff = new string('i', 13);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 12 characters long", result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _tariffRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Tariff", result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }
        #endregion

        #region Port Of Presentation
        [TestMethod]
        public void Validate_ReturnsTrue_IfPortOfPresentationPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.PortOfPresentation = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPortOfPresentationPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.PortOfPresentation = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfPresentationFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.PortOfPresentation = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "PortOfPresentation").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfPresentationIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _domesticPortRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>())).Returns(Enumerable.Empty<DomesticPort>().ToList());

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Port Of Presentation", result.Errors.First(x => x.PropertyName == "PortOfPresentation").ErrorMessage);
        }
        #endregion

        #region ForeignDestination
        [TestMethod]
        public void Validate_ReturnsTrue_IfForeignDestinationPropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.ForeignDestination = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfForeignDestinationPropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.ForeignDestination = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfForeignDestinationFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.ForeignDestination = new string('i', 6);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 5 characters long", result.Errors.First(x => x.PropertyName == "ForeignDestination").ErrorMessage);
        }
        #endregion

        #region Transport Mode
        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModePropertyEmpty()
        {
            ImportModel model = CreateValidModel(x => x.TransportMode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Transport Mode"), result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModePropertyNull()
        {
            ImportModel model = CreateValidModel(x => x.TransportMode = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Transport Mode"),
                result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModeFieldExceedsLength()
        {
            ImportModel model = CreateValidModel(x =>
            {
                x.TransportMode = new string('i', 3);
            });

            _transportModeRepositoryMock.Setup(x => x.GetByNumber(It.IsAny<string>())).Returns(new TransportMode());

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 2 characters long", result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModeIsNotExist()
        {
            ImportModel model = CreateValidModel();

            _transportModeRepositoryMock.Setup(x => x.GetByNumber(It.IsAny<string>())).Returns<TransportMode>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Transport Mode", result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }
        #endregion

        #region Importer Address
        [DataRow(null)]
        [DataRow("")]
        [DataRow("0")]
        [DataRow("1")]
        [DataTestMethod]
        public void Validate_ReturnsTrue_IfConfirmationNeededPropertyIsValid(string value)
        {
            ImportModel model = CreateValidModel(x => x.ConfirmationNeeded = value);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConfirmationNeededPropertyIsInvalid()
        {
            ImportModel model = CreateValidModel(x => x.ConfirmationNeeded = "asd");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Invalid format. Only 0, 1 or empty value is allowed.", result.Errors.First(x => x.PropertyName == "ConfirmationNeeded").ErrorMessage);
        }

        #endregion
    }
}

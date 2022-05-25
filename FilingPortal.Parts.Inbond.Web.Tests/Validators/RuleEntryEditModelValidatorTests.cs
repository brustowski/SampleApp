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
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.Parts.Inbond.Web.Validators;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Parts.Inbond.Web.Tests.Validators
{
    [TestClass]
    public class RuleEntryEditModelValidatorTests
    {
        private RuleEntryEditModelValidator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IFirmsCodesRepository> _firmsCodesRepositoryMock;
        private Mock<IDataProviderRepository<InBondCarrier, string>> _inBondCarrierRepositoryMock;
        private Mock<IDataProviderRepository<EntryType, string>> _entryTypeRepositoryMock;
        private Mock<IDataProviderRepository<DomesticPort, int>> _domesticPortRepositoryMock;
        private Mock<ITariffRepository<HtsTariff>> _tariffRepositoryMock;
        private Mock<ITransportModeRepository> _transportModeRepositoryMock;
        private Mock<IClientAddressRepository> _clientAddressRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new Client());
            _firmsCodesRepositoryMock = new Mock<IFirmsCodesRepository>();
            _firmsCodesRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new CargowiseFirmsCodes());
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
            _clientAddressRepositoryMock = new Mock<IClientAddressRepository>();
            _clientAddressRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new ClientAddress());

            _validator = new RuleEntryEditModelValidator(_clientRepositoryMock.Object
                , _firmsCodesRepositoryMock.Object
                , _inBondCarrierRepositoryMock.Object
                , _entryTypeRepositoryMock.Object
                , _domesticPortRepositoryMock.Object
                , _tariffRepositoryMock.Object
                , _transportModeRepositoryMock.Object
                , _clientAddressRepositoryMock.Object);
        }

        private static RuleEntryEditModel CreateValidModel(Action<RuleEntryEditModel> action = null)
        {
            var model = new RuleEntryEditModel
            {
                Id = 1,
                FirmsCodeId = "1",
                ImporterId = Guid.NewGuid().ToString(),
                Carrier = "TESREFSAT",
                ConsigneeId = Guid.NewGuid().ToString(),
                ConsigneeAddressId = Guid.NewGuid().ToString(),
                UsPortOfDestination = "1108",
                EntryType = "61",
                ShipperId = Guid.NewGuid().ToString(),
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
            RuleEntryEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        #region FIRMs Code
        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodePropertyEmpty()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.FirmsCodeId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "FIRMs Code"),
                result.Errors.First(x => x.PropertyName == "FirmsCodeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodePropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.FirmsCodeId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "FIRMs Code"),
                result.Errors.First(x => x.PropertyName == "FirmsCodeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfFirmsCodeIsNotExist()
        {
            const int id = 1;
            RuleEntryEditModel model = CreateValidModel(x => x.FirmsCodeId = id.ToString());

            _firmsCodesRepositoryMock.Setup(x => x.Get(id)).Returns<CargowiseFirmsCodes>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown FIRMs Code", result.Errors.First(x => x.PropertyName == "FirmsCodeId").ErrorMessage);
        }
        #endregion

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyEmpty()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ImporterId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ImporterId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterHasWrongFormat()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ImporterId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleEntryEditModel model = CreateValidModel(x => x.ImporterId = guid.ToString());

            _clientRepositoryMock.Setup(x => x.Get(guid)).Returns<Client>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer", result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }
        #endregion

        #region Carrier
        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyEmpty()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.Carrier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"), result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.Carrier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"), result.Errors.First(x => x.PropertyName == "Carrier").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCarrierFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

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
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee"),
                result.Errors.First(x => x.PropertyName == "ConsigneeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneePropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Consignee"),
                result.Errors.First(x => x.PropertyName == "ConsigneeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeHasWrongFormat()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ConsigneeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeId = guid.ToString());

            _clientRepositoryMock.Setup(x => x.Get(guid)).Returns<Client>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Consignee", result.Errors.First(x => x.PropertyName == "ConsigneeId").ErrorMessage);
        }
        #endregion

        #region Consignee Address
        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeAddressPropertyEmpty()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeAddressId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeAddressPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeAddressId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfConsigneeAddressHasWrongFormat()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeAddressId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ConsigneeAddressId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfConsigneeAddressIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleEntryEditModel model = CreateValidModel(x => x.ConsigneeAddressId = guid.ToString());

            _clientAddressRepositoryMock.Setup(x => x.Get(guid)).Returns<ClientAddress>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Consignee Address", result.Errors.First(x => x.PropertyName == "ConsigneeAddressId").ErrorMessage);
        }
        #endregion

        #region US Port of Destination
        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationPropertyEmpty()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.UsPortOfDestination = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "US Port Of Destination"), result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.UsPortOfDestination = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "US Port Of Destination"),
                result.Errors.First(x => x.PropertyName == "UsPortOfDestination").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUsPortOfDestinationFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

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
            RuleEntryEditModel model = CreateValidModel(x => x.EntryType = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfEntryTypePropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.EntryType = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfEntryTypeFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

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
            RuleEntryEditModel model = CreateValidModel(x => x.Tariff = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTariffPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.Tariff = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

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
            RuleEntryEditModel model = CreateValidModel(x => x.PortOfPresentation = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPortOfPresentationPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.PortOfPresentation = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfPresentationFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

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
            RuleEntryEditModel model = CreateValidModel(x => x.ForeignDestination = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfForeignDestinationPropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.ForeignDestination = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfForeignDestinationFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel(x => x.TransportMode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Transport Mode"), result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModePropertyNull()
        {
            RuleEntryEditModel model = CreateValidModel(x => x.TransportMode = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Transport Mode"),
                result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportModeFieldExceedsLength()
        {
            RuleEntryEditModel model = CreateValidModel(x =>
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
            RuleEntryEditModel model = CreateValidModel();

            _transportModeRepositoryMock.Setup(x => x.GetByNumber(It.IsAny<string>())).Returns<TransportMode>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Transport Mode", result.Errors.First(x => x.PropertyName == "TransportMode").ErrorMessage);
        }
        #endregion

    }
}

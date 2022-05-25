using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FilingPortal.Parts.Rail.Export.Domain.Tests.Import.Inbound
{
    [TestClass]
    public class InboundImportValidatorTests
    {
        private Validator _validator;
        private Mock<IDomesticPortsRepository> _portRepositoryMock;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IClientAddressRepository> _clientAddressRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _portRepositoryMock = new Mock<IDomesticPortsRepository>();
            _portRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _clientAddressRepositoryMock = new Mock<IClientAddressRepository>();
            _clientAddressRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<ClientAddress>
                    {new ClientAddress {Id = Guid.NewGuid(), Code = "code", ClientId = Guid.NewGuid()}});
            _validator = new Validator(_portRepositoryMock.Object, _clientAddressRepositoryMock.Object, _clientRepositoryMock.Object);
        }

        private static InboundImportModel CreateValidModel(Action<InboundImportModel> action = null)
        {
            var model = new InboundImportModel
            {
                GroupId = "1",
                Exporter = "TITMARCRP",
                Importer = "GRUPETZAP",
                MasterBill = "TIE2000151",
                ContainerNumber = "TILX1234",
                LoadPort = "2361",
                ExportPort = "2304",
                Carrier = "KANSOUMKC",
                TariffType = "HTS",
                Tariff = "2710.19.1102",
                GoodsDescription = "DIESEL FUEL - ULSD",
                CustomsQty = 190.48M,
                Price = 16000M,
                GrossWeight = 25368.780951M,
                GrossWeightUOM = "KG",
                LoadDate = DateTime.Now,
                ExportDate = DateTime.Now,
                TerminalAddress = "HARLINGEN TERMINAL",
                RowNumberInFile = 2
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            InboundImportModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        #region Exporter
        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.Exporter = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Exporter"),
                result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.Exporter = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Exporter"),
                result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.Exporter = new string('i', 13);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 12 characters long", result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExporterIsNotExist()
        {
            InboundImportModel model = CreateValidModel(x => x.Exporter = "A000");

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Exporter Code: A000", result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }
        #endregion

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.Importer = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.Importer = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"),
                result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.Importer = new string('i', 13);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 12 characters long", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIsNotExist()
        {
            InboundImportModel model = CreateValidModel(x => x.Importer = "unknown");

            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Importer Code: unknown", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }
        #endregion

        #region Tariff Type
        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffTypePropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.TariffType = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff Type"),
                result.Errors.First(x => x.PropertyName == "TariffType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffTypePropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.TariffType = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff Type"),
                result.Errors.First(x => x.PropertyName == "TariffType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffTypeFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.TariffType = new string('i', 4);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 3 characters long", result.Errors.First(x => x.PropertyName == "TariffType").ErrorMessage);
        }
        #endregion

        #region Tariff
        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.Tariff = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff"),
                result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.Tariff = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff"),
                result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.Tariff = new string('i', 36);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 35 characters long", result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }
        #endregion

        #region Master Bill
        [TestMethod]
        public void Validate_ReturnsFalse_IfMasterBillPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.MasterBill = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Master Bill"),
                result.Errors.First(x => x.PropertyName == "MasterBill").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfMasterBillPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.MasterBill = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Master Bill"),
                result.Errors.First(x => x.PropertyName == "MasterBill").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfMasterBillFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.MasterBill = new string('i', 21);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 20 characters long", result.Errors.First(x => x.PropertyName == "MasterBill").ErrorMessage);
        }
        #endregion

        #region Load Port
        [TestMethod]
        public void Validate_ReturnsFalse_IfLoadPortFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.LoadPort = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "LoadPort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfLoadPortIsNotExist()
        {
            InboundImportModel model = CreateValidModel(x => x.LoadPort = "0000");

            _portRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Load Port (0000) not recognized", result.Errors.First(x => x.PropertyName == "LoadPort").ErrorMessage);
        }
        #endregion

        #region Export Port
        [TestMethod]
        public void Validate_ReturnsFalse_IfExportPortFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.ExportPort = new string('i', 5);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "ExportPort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportPortIsNotExist()
        {
            InboundImportModel model = CreateValidModel(x => x.ExportPort = "0000");

            _portRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Export Port (0000) not recognized", result.Errors.First(x => x.PropertyName == "ExportPort").ErrorMessage);
        }
        #endregion

        #region Goods Description
        [TestMethod]
        public void Validate_ReturnsFalse_IfGoodsDescriptionPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.GoodsDescription = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Goods Description"),
                result.Errors.First(x => x.PropertyName == "GoodsDescription").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGoodsDescriptionPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.GoodsDescription = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Goods Description"),
                result.Errors.First(x => x.PropertyName == "GoodsDescription").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGoodsDescriptionFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.GoodsDescription = new string('i', 513);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 512 characters long", result.Errors.First(x => x.PropertyName == "GoodsDescription").ErrorMessage);
        }
        #endregion

        #region Gross Weight UOM
        [TestMethod]
        public void Validate_ReturnsFalse_IfGrossWeightUOMPropertyEmpty()
        {
            InboundImportModel model = CreateValidModel(x => x.GrossWeightUOM = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Gross Weight UOM"),
                result.Errors.First(x => x.PropertyName == "GrossWeightUOM").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGrossWeightUOMPropertyNull()
        {
            InboundImportModel model = CreateValidModel(x => x.GrossWeightUOM = null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Gross Weight UOM"),
                result.Errors.First(x => x.PropertyName == "GrossWeightUOM").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGrossWeightUOMFieldExceedsLength()
        {
            InboundImportModel model = CreateValidModel(x =>
            {
                x.GrossWeightUOM = new string('i', 4);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 3 characters long", result.Errors.First(x => x.PropertyName == "GrossWeightUOM").ErrorMessage);
        }
        #endregion

        #region Terminal Address
        [TestMethod]
        public void Validate_ReturnsFalse_IfTerminalAddressIsNotExist()
        {
            InboundImportModel model = CreateValidModel(x => x.TerminalAddress = "unknown address");

            _clientAddressRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<ClientAddress>());

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual($"Unknown address {model.TerminalAddress} for exporter {model.Exporter}", result.Errors.First(x => x.PropertyName == "TerminalAddress").ErrorMessage);
        }
        #endregion
    }
}

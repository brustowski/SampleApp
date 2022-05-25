﻿using System;
using System.Linq;
using FilingPortal.Domain.Imports.TruckExport.RuleExporterConsignee;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Validators.TruckExport
{
    [TestClass]
    public class TruckExportRuleExporterConsigneeValidatorTests
    {
        private Validator _validator;
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IClientAddressRepository> _addressRepositoryMock;
        private Mock<IClientContactsRepository> _contactsRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _addressRepositoryMock = new Mock<IClientAddressRepository>();
            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns(new ClientAddress());
            _contactsRepositoryMock = new Mock<IClientContactsRepository>();
            _contactsRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns(new ClientContact());

            _validator = new Validator(_clientRepositoryMock.Object, _addressRepositoryMock.Object, _contactsRepositoryMock.Object);
        }

        private static ImportModel CreateValidModel(Action<ImportModel> action = null)
        {
            var model = new ImportModel
            {
                Exporter = "TITMARCRP",
                ConsigneeCode = "TRAADUTAM",
                Address = "HARLINGEN TERMINAL",
                Contact = "Jennifer Doerk",
                Phone = "75514821482",
                TranRelated = "Y",
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

        #region Exporter
        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ExporterBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.Exporter = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfExporterNotSpecified()
        {
            ImportModel model = CreateValidModel(x => x.Exporter = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.USPPIIsRequired, result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfExporterExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.Exporter = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128),
                result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfExporterNotExist()
        {
            ImportModel model = CreateValidModel();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidUSPPI, result.Errors.First(x => x.PropertyName == "Exporter").ErrorMessage);
        }
        #endregion

        #region Consignee Code
        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ConsigneeCodeBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.ConsigneeCode = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfConsigneeCodeNotSpecified()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeCode = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.ConsigneeCodeIsRequired, result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfConsigneeCodeExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.ConsigneeCode = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128),
                result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfConsigneeCodeNotExist()
        {
            ImportModel model = CreateValidModel();
            _clientRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidConsignee, result.Errors.First(x => x.PropertyName == "ConsigneeCode").ErrorMessage);
        }
        #endregion

        #region Address
        [DataRow(0, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_AddressBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.Address = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAddressIsNull()
        {
            ImportModel model = CreateValidModel(x => x.Address = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfAddressExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.Address = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128),
                result.Errors.First(x => x.PropertyName == "Address").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfAddressNotExist()
        {
            ImportModel model = CreateValidModel();
            _addressRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientAddress>(null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidAddress, result.Errors.First(x => x.PropertyName == "Address").ErrorMessage);
        }
        #endregion

        #region Contact
        [DataRow(0, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ContactBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.Contact = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfContactIsNull()
        {
            ImportModel model = CreateValidModel(x => x.Contact = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfContactExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.Contact = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128),
                result.Errors.First(x => x.PropertyName == "Contact").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfContactNotExist()
        {
            ImportModel model = CreateValidModel();
            _contactsRepositoryMock.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ClientContact>(null);
            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidContact, result.Errors.First(x => x.PropertyName == "Contact").ErrorMessage);
        }
        #endregion

        #region Phone
        [DataRow(0, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_PhoneBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.Phone = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPhoneIsNull()
        {
            ImportModel model = CreateValidModel(x => x.Phone = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfPhoneExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.Phone = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128),
                result.Errors.First(x => x.PropertyName == "Phone").ErrorMessage);
        }
        #endregion

        #region TranRelated
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(2, false)]
        [DataTestMethod]
        public void Validate_TranRelatedBoundaryValues(int length, bool expectedResult)
        {
            ImportModel model = CreateValidModel(m => m.TranRelated = new string('i', length));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTranRelatedIsNull()
        {
            ImportModel model = CreateValidModel(x => x.TranRelated = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfTranRelatedExceedsLength()
        {
            ImportModel model = CreateValidModel(x => x.TranRelated = new string('i', 2));

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 1),
                result.Errors.First(x => x.PropertyName == "TranRelated").ErrorMessage);
        }
        #endregion
    }
}

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
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Web.Tests.Validators.Rail
{
    [TestClass]
    public class RailRuleDescriptionEditModelValidatorTests
    {
        private RailRuleDescriptionEditModelValidator _validator;
        private Mock<ILookupMasterRepository<LookupMaster>> _portRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _portRepositoryMock = new Mock<ILookupMasterRepository<LookupMaster>>();
            _portRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(true);
            _validator = new RailRuleDescriptionEditModelValidator(_portRepositoryMock.Object);
        }

        private RailRuleDescriptionEditModel CreateValidModel(Action<RailRuleDescriptionEditModel> action = null)
        {
            var model = new RailRuleDescriptionEditModel
            {
                Id = 1,
                Description1 = "Description1",
                Importer = "Importer",
                Supplier = "Supplier",
                Port = "Port",
                ProductID = "ProductID",
                Attribute1 = "Attribute",
                Tariff = "Tariff",
                GoodsDescription = "GoodsDescription",
                InvoiceUOM = "InvoiceUOM",
                TemplateHTSQuantity = "13.47",
                TemplateInvoiceQuantity = "13.47",
                Destination = "LA"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RailRuleDescriptionEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        #region Description1
        [TestMethod]
        public void Validate_ReturnsFalse_IfDescription1Empty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Description1 = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Description 1 is required", result.Errors.First(x => x.PropertyName == "Description1").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDescription1Null()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Description1 = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Description 1 is required", result.Errors.First(x => x.PropertyName == "Description1").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfDescription1NotExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Description1 = new string('i', 128));

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public void Validate_ReturnsFalse_IfDescription1ExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Description1 = new string('i', 501));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 500 characters long", result.Errors.First(x => x.PropertyName == "Description1").ErrorMessage);
        }
        #endregion

        #region Importer
        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Importer = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterNull()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Importer = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Importer = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }
        #endregion

        #region Supplier

        [TestMethod] public void Validate_ReturnsFalse_IfSupplierEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Supplier = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierNull()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Supplier = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSupplierExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Supplier = new string('i', 129));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 128 characters long", result.Errors.First(x => x.PropertyName == "Supplier").ErrorMessage);
        }
        #endregion

        [TestMethod]
        public void Validate_ReturnsTrue_IfImporterAndSupplierEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.Importer = string.Empty;
                x.Supplier = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        #region Port
        [TestMethod]
        public void Validate_ReturnsFalse_IfPortEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Port = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortNull()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Port = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfPortAndDestinationEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.Port = string.Empty;
                x.Destination = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.Port = new string('i', 5));

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 4 characters long", result.Errors.First(x => x.PropertyName == "Port").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortIsNotExist()
        {
            RailRuleDescriptionEditModel model = CreateValidModel();

            _portRepositoryMock.Setup(x => x.IsExist(It.IsAny<string>())).Returns(false);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.InvalidPort, result.Errors.First(x => x.PropertyName == "Port").ErrorMessage);
        }
        #endregion

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.ProductID = string.Empty;
                x.Attribute1 = string.Empty;
                x.GoodsDescription = string.Empty;
                x.InvoiceUOM = string.Empty;
                x.TemplateHTSQuantity = string.Empty;
                x.TemplateInvoiceQuantity = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.ProductID = null;
                x.Attribute1 = null;
                x.GoodsDescription = null;
                x.InvoiceUOM = null;
                x.TemplateHTSQuantity = null;
                x.TemplateInvoiceQuantity = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableStringFieldsNotExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.ProductID = new string('i', 128);
                x.Attribute1 = new string('i', 128);
                x.Tariff = new string('i', 128);
                x.GoodsDescription = new string('i', 128);
                x.InvoiceUOM = new string('i', 128);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfNullableStringFieldsExceedsLength()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x =>
            {
                x.ProductID = new string('i', 129);
                x.Attribute1 = new string('i', 129);
                x.Tariff = new string('i', 129);
                x.GoodsDescription = new string('i', 129);
                x.InvoiceUOM = new string('i', 129);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(5, result.Errors.Count);
            Assert.IsTrue(result.Errors.All(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateHTSQuantityIsCorrectDecimalWithoutPoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateHTSQuantity = "12345");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateHTSQuantityIsCorrectDecimalWithPoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateHTSQuantity = "123456.55");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateHTSQuantityIsDecimalWith12NumbersBeforePoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateHTSQuantity = "123456789012.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTemplateHTSQuantityIsDecimalWithMoreThan17NumbersBeforePoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateHTSQuantity = "123343434354445698.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "TemplateHTSQuantity").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTemplateHTSQuantityIsDecimalWithLetters()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateHTSQuantity = "34dfsd.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "TemplateHTSQuantity").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateInvoiceQuantityIsCorrectDecimalWithoutPoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateInvoiceQuantity = "12345");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateInvoiceQuantityIsCorrectDecimalWithPoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateInvoiceQuantity = "123456.55");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfTemplateInvoiceQuantityIsDecimalWith12NumbersBeforePoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateInvoiceQuantity = "123456789012.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTemplateInvoiceQuantityIsDecimalWithMoreThan12NumbersBeforePoint()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateInvoiceQuantity = "123343434354445698.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "TemplateInvoiceQuantity").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTemplateInvoiceQuantityIsDecimalWithLetters()
        {
            RailRuleDescriptionEditModel model = CreateValidModel(x => x.TemplateInvoiceQuantity = "34dfsd.08");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.DecimalFormatMismatch, result.Errors.First(x => x.PropertyName == "TemplateInvoiceQuantity").ErrorMessage);
        }
    }
}

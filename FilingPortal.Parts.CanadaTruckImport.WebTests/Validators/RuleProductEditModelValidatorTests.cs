using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleProduct;
using FilingPortal.Parts.CanadaTruckImport.Web.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Parts.CanadaTruckImport.WebTests.Validators
{
    [TestClass]
    public class RuleProductEditModelValidatorTests
    {
        private RuleProductEditModelValidator _validator;
        private Mock<IProductCodeRepository> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IProductCodeRepository>();
            _repositoryMock.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new ProductCode());
            _validator = new RuleProductEditModelValidator(_repositoryMock.Object);
        }

        private static RuleProductEditModel CreateValidModel(Action<RuleProductEditModel> action = null)
        {
            var model = new RuleProductEditModel
            {
                Id = 1,
                ProductCodeId = Guid.NewGuid().ToString(),
                GrossWeightUnit = "LB",
                PackagesUnit = "VL",
                InvoiceUQ = "GA"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RuleProductEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RuleProductEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        #region Product
        [TestMethod]
        public void Validate_ReturnsFalse_IfProductPropertyEmpty()
        {
            RuleProductEditModel model = CreateValidModel(x => x.ProductCodeId = string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Product"), result.Errors.First(x => x.PropertyName == "ProductCodeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfProductPropertyNull()
        {
            RuleProductEditModel model = CreateValidModel(x => x.ProductCodeId = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Product"), result.Errors.First(x => x.PropertyName == "ProductCodeId").ErrorMessage);
        }
        [TestMethod]
        public void Validate_ReturnsFalse_IfProductCodeIdHasWrongFormat()
        {
            RuleProductEditModel model = CreateValidModel(x => x.ProductCodeId = "wrong guid");

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch,
                result.Errors.First(x => x.PropertyName == "ProductCodeId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfProductCodeIdIsNotExist()
        {
            var guid = Guid.NewGuid();
            RuleProductEditModel model = CreateValidModel(x => x.ProductCodeId = guid.ToString());

            _repositoryMock.Setup(x => x.Get(guid)).Returns<ProductCode>(null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unknown Product", result.Errors.First(x => x.PropertyName == "ProductCodeId").ErrorMessage);
        }
        #endregion

        #region GrossWeightUnit
        [TestMethod]
        public void Validate_ReturnsFalse_IfGrossWeightUnitIsNull()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.GrossWeightUnit = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(3, true)]
        [DataRow(4, false)]
        [DataTestMethod]
        public void Validate_GrossWeightUnitBoundaryValues(int length, bool expectedResult)
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.GrossWeightUnit = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGrossWeightUnitFieldExceedsLength()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.GrossWeightUnit = new string('i', 4);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 3 characters long"
                , result.Errors.First(x => x.PropertyName == nameof(RuleProductEditModel.GrossWeightUnit)).ErrorMessage);
        }
        #endregion

        #region PackagesUnit
        [TestMethod]
        public void Validate_ReturnsFalse_IfPackagesUnitIsNull()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.PackagesUnit = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(3, true)]
        [DataRow(4, false)]
        [DataTestMethod]
        public void Validate_PackagesUnitBoundaryValues(int length, bool expectedResult)
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.PackagesUnit = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPackagesUnitFieldExceedsLength()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.PackagesUnit = new string('i', 4);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 3 characters long"
                , result.Errors.First(x => x.PropertyName == nameof(RuleProductEditModel.PackagesUnit)).ErrorMessage);
        }
        #endregion

        #region InvoiceUQ
        [TestMethod]
        public void Validate_ReturnsFalse_IfInvoiceUQIsNull()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.InvoiceUQ = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(3, true)]
        [DataRow(4, false)]
        [DataTestMethod]
        public void Validate_InvoiceUQBoundaryValues(int length, bool expectedResult)
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.InvoiceUQ = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfInvoiceUQFieldExceedsLength()
        {
            RuleProductEditModel model = CreateValidModel(x =>
            {
                x.InvoiceUQ = new string('i', 4);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 3 characters long"
                , result.Errors.First(x => x.PropertyName == nameof(RuleProductEditModel.InvoiceUQ)).ErrorMessage);
        }
        #endregion
    }
}

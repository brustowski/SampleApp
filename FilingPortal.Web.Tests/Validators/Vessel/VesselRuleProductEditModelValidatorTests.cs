using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.Validators.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.Vessel
{
    [TestClass]
    public class VesselRuleProductEditModelValidatorTests
    {
        private VesselRuleProductEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new VesselRuleProductEditModelValidator();
        }

        private VesselRuleProductEditModel CreateValidModel(Action<VesselRuleProductEditModel> action = null)
        {
            var model = new VesselRuleProductEditModel
            {
                Id = 1,
                Tariff = "Tariff",
                GoodsDescription = "GoodsDescription",
                CustomsAttribute1 = "CustomsAttribute1",
                CustomsAttribute2 = "CustomsAttribute2",
                InvoiceUQ = "InvoiceUQ",
                TSCARequirement = "TSCARequirement"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            VesselRuleProductEditModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            VesselRuleProductEditModel model = CreateValidModel(x =>
            {
                x.GoodsDescription = string.Empty;
                x.CustomsAttribute1 = string.Empty;
                x.CustomsAttribute2 = string.Empty;
                x.InvoiceUQ = string.Empty;
                x.TSCARequirement = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            VesselRuleProductEditModel model = CreateValidModel(x =>
            {
                x.GoodsDescription = null;
                x.CustomsAttribute1 = null;
                x.CustomsAttribute2 = null;
                x.InvoiceUQ = null;
                x.TSCARequirement = null;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            VesselRuleProductEditModel model = CreateValidModel(x => x.Id = -1);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRequiredPropertyEmpty()
        {
            VesselRuleProductEditModel model = CreateValidModel(x => x.Tariff = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.TariffIsRequired, result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRequiredPropertyNull()
        {
            VesselRuleProductEditModel model = CreateValidModel(x => x.Tariff = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.TariffIsRequired, result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [DataRow(1, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues128(int length, bool expectedResult)
        {
            VesselRuleProductEditModel model = CreateValidModel(x =>
            {
                x.GoodsDescription = new string('i', length);
                x.CustomsAttribute1 = new string('i', length);
                x.CustomsAttribute2 = new string('i', length);
                x.InvoiceUQ = new string('i', length);
                x.TSCARequirement = new string('i', length);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [DataRow(1, true)]
        [DataRow(10, true)]
        [DataRow(11, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues10(int length, bool expectedResult)
        {
            VesselRuleProductEditModel model = CreateValidModel(x =>
            {
                x.Tariff = new string('i', length);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength10()
        {
            VesselRuleProductEditModel model = CreateValidModel(x =>
            {
                x.Tariff = new string('i', 11);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 10 characters long"));
        }
    }
}

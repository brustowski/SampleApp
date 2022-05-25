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
    public class VesselRuleImporterEditModelValidatorTests
    {
        private VesselRuleImporterEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new VesselRuleImporterEditModelValidator();
        }

        private VesselRuleImporterEditModel CreateValidModel(Action<VesselRuleImporterEditModel> action = null)
        {
            var model = new VesselRuleImporterEditModel
            {
                Id = 1,
                Importer = "Importer",
                CWImporter = "CWImporter",
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            VesselRuleImporterEditModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.CWImporter = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.CWImporter = null;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x => x.Id = -1);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRequiredPropertyEmpty()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x => x.Importer = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ImporterIsRequired, result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRequiredPropertyNull()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x => x.Importer = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.ImporterIsRequired, result.Errors.First(x => x.PropertyName == "Importer").ErrorMessage);
        }

        [DataRow(1, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues128(int length, bool expectedResult)
        {
            VesselRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.Importer = new string('i', length);
                x.CWImporter = new string('i', length);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength128()
        {
            VesselRuleImporterEditModel model = CreateValidModel(x =>
            {
                x.Importer = new string('i', 129);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }
    }
}

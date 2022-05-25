using System;
using System.Linq;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.Validators.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.Vessel
{
    [TestClass]
    public class VesselRulePortEditModelValidatorTests
    {
        private VesselRulePortEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new VesselRulePortEditModelValidator();
        }

        private VesselRulePortEditModel CreateValidModel(Action<VesselRulePortEditModel> action = null)
        {
            var model = new VesselRulePortEditModel
            {
                Id = 1,
                EntryPort = "Port",
                DischargePort = "Port",
                FirmsCode = "FIRMsCode",
                FirmsCodeId = "123",
                HMF = "Y"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            VesselRulePortEditModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            VesselRulePortEditModel model = CreateValidModel(x =>
            {
                x.EntryPort = string.Empty;
                x.DischargePort = string.Empty;
                x.FirmsCode = string.Empty;
                x.HMF = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            VesselRulePortEditModel model = CreateValidModel(x =>
            {
                x.EntryPort = null;
                x.DischargePort = null;
                x.FirmsCode = null;
                x.HMF = null;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            VesselRulePortEditModel model = CreateValidModel(x => x.Id = -1);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [DataRow(1, true)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues128(int length, bool expectedResult)
        {
            VesselRulePortEditModel model = CreateValidModel(x =>
            {
                x.FirmsCode = new string('i', length);
                x.HMF = new string('i', length);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [DataRow(1, true)]
        [DataRow(4, true)]
        [DataRow(5, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues4(int length, bool expectedResult)
        {
            VesselRulePortEditModel model = CreateValidModel(x =>
            {
                x.EntryPort = new string('i', length);
                x.DischargePort = new string('i', length);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength4()
        {
            VesselRulePortEditModel model = CreateValidModel(x =>
            {
                x.EntryPort = new string('i', 5);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 4 characters long"));
        }
    }
}

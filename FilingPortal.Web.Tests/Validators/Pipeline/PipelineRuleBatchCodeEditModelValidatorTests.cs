using System;
using System.Linq;
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.Validators.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.Pipeline
{
    [TestClass]
    public class PipelineRuleBatchCodeEditModelValidatorTests
    {
        private PipelineRuleBatchCodeEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new PipelineRuleBatchCodeEditModelValidator();
        }

        private PipelineRuleBatchCodeEditModel CreateValidModel(Action<PipelineRuleBatchCodeEditModel> action = null)
        {
            var model = new PipelineRuleBatchCodeEditModel
            {
                Id = 1,
                BatchCode = "BatchCode",
                Product = "Product"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            var model = CreateValidModel();

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNullOrEmpty()
        {
            var model = new PipelineRuleBatchCodeEditModel
            {
                Id = 1,
                BatchCode = "BatchCode",
            };

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            var model = CreateValidModel(x => x.Id = -1);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithEmptyRequiredFields()
        {
            var model = CreateValidModel(x => x.BatchCode = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithNullRequiredFields()
        {
            var model = CreateValidModel(x => x.BatchCode = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_StringBoundaryValues(int length, bool expectedResult)
        {
            var model = CreateValidModel(x =>
            {
                x.BatchCode = new string('i', length);
                x.Product = new string('i', length);
            });

            var result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfStringFieldExceedsLength()
        {
            var model = CreateValidModel(x =>
            {
                x.BatchCode = new string('i', 129);
            });

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }
    }
}

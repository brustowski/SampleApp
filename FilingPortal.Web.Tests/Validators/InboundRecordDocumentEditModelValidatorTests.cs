using System;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators
{
    [TestClass]
    public class InboundRecordDocumentEditModelValidatorTests
    {
        private InboundRecordDocumentEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new InboundRecordDocumentEditModelValidator();
        }

        private InboundRecordDocumentEditModel CreateValidModel(Action<InboundRecordDocumentEditModel> action = null)
        {
            var model = new InboundRecordDocumentEditModel
            {
                Name = "Document name",
                Type = "Document type"
            };

            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_ForValidModel()
        {
            var model = CreateValidModel();

            var result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithEmptyFileName()
        {
            var model = CreateValidModel(x => x.Name = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithNullFileName()
        {
            var model = CreateValidModel(x => x.Name = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithEmptyType()
        {
            var model = CreateValidModel(x => x.Type = string.Empty);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_WithNullType()
        {
            var model = CreateValidModel(x => x.Type = null);

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }
    }
}

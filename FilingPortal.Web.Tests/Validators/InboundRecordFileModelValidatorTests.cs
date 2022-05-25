using System;
using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators
{
    [TestClass]
    public class InboundRecordFileModelValidatorTests
    {
        private InboundRecordFileModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new InboundRecordFileModelValidator();
        }

        private InboundRecordFileModel CreateValidModel(Action<InboundRecordFileModel> action = null)
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 3,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel()
                    {
                        Id = 34,
                        Value = "Value"
                    }
                },
                Documents = new List<InboundRecordDocumentEditModel>
                {
                    new InboundRecordDocumentEditModel
                    {
                        Name = "Document name",
                        Type = "Document type"
                    }
                }
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
        public void Validate_ReturnsFalse_WithEmptyFilingHeader()
        {
            var model = CreateValidModel(x => x.FilingHeaderId = default(int));

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }
        
        [TestMethod]
        public void Validate_ReturnsFalse_WithNotValidDocumentsProperty()
        {
            var model = CreateValidModel(x => x.Documents = new List<InboundRecordDocumentEditModel>
            {
                new InboundRecordDocumentEditModel { Name = "error" }
            });

            var result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }
    }
}

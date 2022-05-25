using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Imports.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Tests.Validators.Pipeline
{
    [TestClass]
    public class PipelineInboundParsingDataModelValidatorTests
    {
        private PipelineInboundParsingDataModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new PipelineInboundParsingDataModelValidator();
        }

        private static PipelineInboundImportParsingModel CreateValidModel(Action<PipelineInboundImportParsingModel> action = null)
        {
            var model = new PipelineInboundImportParsingModel
            {
                Importer = "Importer",
                Batch = "Batch",
                TicketNumber = "TicketNumber",
                Quantity = 123456789012.123456M,
                API = 123456789012.123456M,
                ExportDate = DateTime.Now,
                ImportDate = DateTime.Now,
                SiteName = "SiteName",
                Facility = "Facility",
                RowNumberInFile = 1,
                EntryNumber = "EntryNumber"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            PipelineInboundImportParsingModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(201, false)]
        [DataRow(200, true)]
        [DataTestMethod]
        public void Validate_ImporterBoundaryValues(int length, bool expectedResult)
        {
            PipelineInboundImportParsingModel model = CreateValidModel(m => m.Importer = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfImporterNotSpecified()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.Importer = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.ImporterIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfImporterExceedsLength()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.Importer = new string('i', 201));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 200), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(21, false)]
        [DataRow(20, true)]
        [DataTestMethod]
        public void Validate_BatchBoundaryValues(int length, bool expectedResult)
        {
            PipelineInboundImportParsingModel model = CreateValidModel(m => m.Batch = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfBatchNotSpecified()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.Batch = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.BatchIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfBatchExceedsLength()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.Batch = new string('i', 21));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(21, false)]
        [DataRow(20, true)]
        [DataTestMethod]
        public void Validate_TicketNumberBoundaryValues(int length, bool expectedResult)
        {
            PipelineInboundImportParsingModel model = CreateValidModel(m => m.TicketNumber = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }


        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfTicketNumberNotSpecified()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.TicketNumber = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.TicketNumberIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfTicketNumberExceedsLength()
        {
            PipelineInboundImportParsingModel model = CreateValidModel(x => x.TicketNumber = new string('i', 21));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20), result.Errors.First().ErrorMessage);
        }
    }
}

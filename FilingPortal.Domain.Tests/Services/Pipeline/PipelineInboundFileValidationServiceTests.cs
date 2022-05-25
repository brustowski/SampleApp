using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Services.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data;
using FilingPortal.Parts.Common.Domain.Services.DB;
using FluentValidation;
using FluentValidation.Results;

namespace FilingPortal.Domain.Tests.Services.Pipeline
{
    [TestClass()]
    public class PipelineInboundFileValidationServiceTests : TestWithApplicationMapping
    {
        private PipelineInboundFileValidationService _service;
        private Mock<IParsingDataModelValidatorFactory> _validatorFactoryMock;
        private Mock<IParseModelMapRegistry> _parseModelMapRegistryMock;
        private Mock<ISqlQueryExecutor> _sqlQueryExecutorMock;
        private Mock<IValidator<PipelineInboundImportParsingModel>> _validator;

        [TestInitialize]
        public void Init()
        {
            _validatorFactoryMock = new Mock<IParsingDataModelValidatorFactory>();
            _parseModelMapRegistryMock = new Mock<IParseModelMapRegistry>();
            _sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();

            var parseModel = new Mock<IParseModelMap>();
            _parseModelMapRegistryMock.Setup(x => x.Get<PipelineInboundImportParsingModel>()).Returns(parseModel.Object);

            _validator = new Mock<IValidator<PipelineInboundImportParsingModel>>();
            SetValidatorResult(true);
            
            _validatorFactoryMock.Setup(x => x.Create<PipelineInboundImportParsingModel>()).Returns(_validator.Object);

            _sqlQueryExecutorMock.Setup(x => x.ExecuteSqlQuery(It.IsAny<string>())).Returns(new DataTable());

            _service = new PipelineInboundFileValidationService(_validatorFactoryMock.Object, _parseModelMapRegistryMock.Object, _sqlQueryExecutorMock.Object);
        }

        private void SetValidatorResult(bool isValid)
        {
            var result = new ValidationResult();
            if (!isValid)
                result.Errors.Add(new ValidationFailure("property", "error message"));

            _validator.Setup(x => x.Validate(It.IsAny<PipelineInboundImportParsingModel>()))
                .Returns(result);
        }

        [TestMethod()]
        public void Validate_Without_Parsing_Model_Returns_Exception()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>();

            _parseModelMapRegistryMock.Setup(x => x.Get<PipelineInboundImportParsingModel>())
                .Returns((IParseModelMap) null);

            Assert.ThrowsException<FileParserException>(() => _service.Validate(records), "Parsing model not found");
        }

        [TestMethod()]
        public void Validate_Without_Validator_Returns_Exception()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>();

            _validatorFactoryMock.Setup(x => x.Create<PipelineInboundImportParsingModel>()).Returns((IValidator<PipelineInboundImportParsingModel>)null);

            Assert.ThrowsException<FileParserException>(() => _service.Validate(records), "Validator not found");
        }

        [TestMethod()]
        public void Validate_Without_Records_Returns_Valid()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>();

            var result = _service.Validate(records);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod()]
        public void Validate_Null_Returns_True()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>()
            {
                null
            };

            SetValidatorResult(true);

            var result = _service.Validate(records);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod()]
        public void Validate_One_Record_Returns_True()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>()
            {
                new PipelineInboundImportParsingModel()
            };

            SetValidatorResult(true);

            var result = _service.Validate(records);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod()]
        public void Validate_Two_Same_Records_Returns_False()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>()
            {
                new PipelineInboundImportParsingModel(),
                new PipelineInboundImportParsingModel(),
            };

            SetValidatorResult(true);

            var result = _service.Validate(records);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod()]
        public void Validate_Invalid_Record_Returns_False()
        {
            IEnumerable<PipelineInboundImportParsingModel> records = new List<PipelineInboundImportParsingModel>()
            {
                new PipelineInboundImportParsingModel(),
            };

            SetValidatorResult(false);

            var result = _service.Validate(records);

            Assert.IsFalse(result.IsValid);
        }


    }
}
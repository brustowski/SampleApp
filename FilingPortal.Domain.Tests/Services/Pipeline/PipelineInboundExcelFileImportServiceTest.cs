using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Services.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Domain.Tests.Services.Pipeline
{
    [TestClass]
    public class PipelineInboundExcelFileImportServiceTest : TestWithApplicationMapping
    {
        private PipelineInboundExcelFileImportService _service;
        private Mock<IFileParser> _fileParserMock;
        private Mock<IParsingDataValidationService<PipelineInboundImportParsingModel>> _validationServiceMock;
        private Mock<IPipelineInboundRepository> _repositoryMock;
        private const string FileName = "test.file";
        private const string FilePath = "test path";

        [TestInitialize]
        public void TestInitialize()
        {
            _fileParserMock = new Mock<IFileParser>();
            _fileParserMock.Setup(x => x.Parse<PipelineInboundImportParsingModel>(It.IsAny<string>())).Returns(new ParsingResult<PipelineInboundImportParsingModel>());
            _validationServiceMock = new Mock<IParsingDataValidationService<PipelineInboundImportParsingModel>>();
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<PipelineInboundImportParsingModel>>())).Returns(new ParsedDataValidationResult<PipelineInboundImportParsingModel>());
            _repositoryMock = new Mock<IPipelineInboundRepository>();
            _service = new PipelineInboundExcelFileImportService(_fileParserMock.Object, _validationServiceMock.Object, _repositoryMock.Object);
        }


        public void Process_ReturnValidResult()
        {
            FileProcessingResult result = _service.Process(FileName, FilePath);
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(FileName, result.FileName);
        }

        [TestMethod]
        public void Process_CallsParserService()
        {
            _service.Process(FileName, FilePath);
            _fileParserMock.Verify(x => x.Parse<PipelineInboundImportParsingModel>(It.Is<string>(v => v.Equals(FilePath))), Times.Once);
        }

        [TestMethod]
        public void Process_ReturnsResultWithCommonErrors_WhenParseServiceReturnsErrors()
        {
            var parsingResult = new ParsingResult<PipelineInboundImportParsingModel>(new[] { "error 1", "error 2" });
            _fileParserMock.Setup(x => x.Parse<PipelineInboundImportParsingModel>(It.IsAny<string>())).Returns(parsingResult);

            FileProcessingResult result = _service.Process(FileName, FilePath);

            Assert.AreEqual(2, result.CommonErrors.Count);
            Assert.IsTrue(result.CommonErrors.Contains("error 1"));
            Assert.IsTrue(result.CommonErrors.Contains("error 2"));
        }

        [TestMethod]
        public void Process_CallsValidationService()
        {
            _service.Process(FileName, FilePath);

            _validationServiceMock.Verify(x => x.Validate(It.IsAny<IEnumerable<PipelineInboundImportParsingModel>>()), Times.Once);
        }

        [TestMethod]
        public void Process_ReturnsResultWithValidationErrors_WhenValidationServiceReturnsErrors()
        {
            var validationResult = new ParsedDataValidationResult<PipelineInboundImportParsingModel>();
            validationResult.AddError(new[]
            {
                new RowError(1,"", ErrorLevel.Critical,"", "Validation error"),
                new RowError(2,"", ErrorLevel.Critical,"", "Validation error")
            });
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<PipelineInboundImportParsingModel>>())).Returns(validationResult);

            FileProcessingResult result = _service.Process(FileName, FilePath);

            Assert.AreEqual(2, result.ValidationErrors.Count);
        }

        [TestMethod]
        public void Process_CallsRepository_WithValidData()
        {
            var validationResult = new ParsedDataValidationResult<PipelineInboundImportParsingModel>();
            validationResult.AddData(new[]
            {
                new PipelineInboundImportParsingModel(),
                new PipelineInboundImportParsingModel()
            });
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<PipelineInboundImportParsingModel>>())).Returns(validationResult);

            _service.Process(FileName, FilePath);

            _repositoryMock.Verify(x => x.Add(It.IsAny<PipelineInbound>()), Times.Exactly(2));
            _repositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Process_DoesNotCallsRepository_WithInvalidData()
        {
            _service.Process(FileName, FilePath);

            _repositoryMock.Verify(x => x.Add(It.IsAny<PipelineInbound>()), Times.Never);
            _repositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}

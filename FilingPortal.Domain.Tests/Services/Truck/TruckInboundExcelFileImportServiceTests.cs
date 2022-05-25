using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Imports.Truck.Inbound;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Domain.Services.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Domain.Tests.Services.Truck
{
    [TestClass]
    public class TruckInboundExcelFileImportServiceTests : TestWithApplicationMapping
    {
        private TruckInboundExcelFileImportService _service;

        private Mock<IFileParser> _fileParserMock;
        private Mock<IParsingDataValidationService<ImportModel>> _validationServiceMock;
        private Mock<ITruckInboundRepository> _repositoryMock;
        private const string FileName = "test.file";
        private const string FilePath = "test path";

        [TestInitialize]
        public void TestInitialize()
        {
            _fileParserMock = new Mock<IFileParser>();
            _fileParserMock.Setup(x => x.Parse<ImportModel>(It.IsAny<string>())).Returns(new ParsingResult<ImportModel>());
            _validationServiceMock = new Mock<IParsingDataValidationService<ImportModel>>();
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<ImportModel>>())).Returns(new ParsedDataValidationResult<ImportModel>());
            _repositoryMock = new Mock<ITruckInboundRepository>();
            _service = new TruckInboundExcelFileImportService(_fileParserMock.Object, _validationServiceMock.Object, _repositoryMock.Object);
        }

        [TestMethod]
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
            _fileParserMock.Verify(x => x.Parse<ImportModel>(It.Is<string>(v => v.Equals(FilePath))), Times.Once);
        }

        [TestMethod]
        public void Process_ReturnsResultWithCommonErrors_WhenParseServiceReturnsErrors()
        {
            var parsingResult = new ParsingResult<ImportModel>(new[] { "error 1", "error 2" });
            _fileParserMock.Setup(x => x.Parse<ImportModel>(It.IsAny<string>())).Returns(parsingResult);

            FileProcessingResult result = _service.Process(FileName, FilePath);

            Assert.AreEqual(2, result.CommonErrors.Count);
            Assert.IsTrue(result.CommonErrors.Contains("error 1"));
            Assert.IsTrue(result.CommonErrors.Contains("error 2"));
        }

        [TestMethod]
        public void Process_CallsValidationService()
        {
            _service.Process(FileName, FilePath);
            _validationServiceMock.Verify(x => x.Validate(It.IsAny<IEnumerable<ImportModel>>()), Times.Once);
        }

        [TestMethod]
        public void Process_ReturnsResultWithValidationErrors_WhenValidationServiceReturnsErrors()
        {
            var validationResult = new ParsedDataValidationResult<ImportModel>();
            validationResult.AddError(new[]
            {
                new RowError(1,"", ErrorLevel.Critical,"", "Validation error"),
                new RowError(2,"", ErrorLevel.Critical,"", "Validation error")
            });
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<ImportModel>>())).Returns(validationResult);

            FileProcessingResult result = _service.Process(FileName, FilePath);

            Assert.AreEqual(2, result.ValidationErrors.Count);
        }

        [TestMethod]
        public void Process_CallsRepository_WithValidData()
        {
            var validationResult = new ParsedDataValidationResult<ImportModel>();
            validationResult.AddData(new[]
            {
                new ImportModel(),
                new ImportModel()
            });
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<ImportModel>>())).Returns(validationResult);

            _service.Process(FileName, FilePath);

            _repositoryMock.Verify(x => x.Add(It.IsAny<TruckInbound>()), Times.Exactly(2));
            _repositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Process_DoesNotCallsRepository_WithInvalidData()
        {
            var validationResult = new ParsedDataValidationResult<ImportModel>();
            _validationServiceMock.Setup(x => x.Validate(It.IsAny<IEnumerable<ImportModel>>())).Returns(validationResult);

            _service.Process(FileName, FilePath);

            _repositoryMock.Verify(x => x.Add(It.IsAny<TruckInbound>()), Times.Never);
            _repositoryMock.Verify(x => x.Save(), Times.Never);
        }
    }
}

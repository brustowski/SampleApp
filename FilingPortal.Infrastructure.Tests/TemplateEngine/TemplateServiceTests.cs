using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;
using FilingPortal.Infrastructure.TemplateEngine;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace FilingPortal.Infrastructure.Tests.TemplateEngine
{
    [TestClass]
    public class TemplateServiceTests
    {
        #region Setup

        private TemplateService _service;

        private Mock<IParseModelMapRegistry> _registryMock;
        private Mock<IReporterFactory> _excelReporterFactoryMock;
        private Mock<IReporter> _excelReporterMock;
        private Mock<IParseModelMap> _modelMapMock;

        private const string SavedFilePath = "savedFilePath";

        private Mock<IImportConfiguration> _config;

        [TestInitialize]
        public void Init()
        {
            _registryMock = new Mock<IParseModelMapRegistry>();
            _excelReporterFactoryMock = new Mock<IReporterFactory>();
            _excelReporterMock = new Mock<IReporter>();
            _modelMapMock = new Mock<IParseModelMap>();

            _service = new TemplateService(
                _registryMock.Object,
                _excelReporterFactoryMock.Object);

            _config = new Mock<IImportConfiguration>();

            _registryMock.Setup(x => x.Get(It.IsAny<Type>())).Returns(_modelMapMock.Object);
            _excelReporterFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(_excelReporterMock.Object);
            _excelReporterMock.Setup(x => x.SaveToFile()).Returns(SavedFilePath);
        }

        #endregion

        [TestMethod]
        public void Create_Returns_ValidResult()
        {
            // Arrange 
            const string fileName = "test";
            _config.Setup(x => x.FileName).Returns(fileName);

            // Act
            FileExportResult result = _service.Create(_config.Object);

            // Assert
            result.FileName.ShouldBeEqualTo(SavedFilePath);
            result.DocumentExternalName.ShouldBeEqualTo($"{fileName}.xlsx");
        }

        [TestMethod]
        public void Create_CallsFactoryMethod_Create()
        {
            const string fileName = "test";
            const string sheetName = "test";
            _config.Setup(x => x.FileName).Returns(fileName);
            _modelMapMock.Setup(x => x.SheetName).Returns(sheetName);

            _service.Create(_config.Object);

            _excelReporterFactoryMock.Verify(x => x.Create($"{fileName}.xlsx", sheetName), Times.Once);
        }

        [TestMethod]
        public void Create_CallsReport_GetHeaderRows()
        {
            _service.Create(_config.Object);

            _excelReporterMock.Verify(x => x.AddPartOfData(It.IsAny<IEnumerable<Row>>()), Times.Once);
        }

    }
}

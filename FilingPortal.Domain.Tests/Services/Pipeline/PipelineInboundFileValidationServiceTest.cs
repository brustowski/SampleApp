using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Imports.Pipeline;
using FilingPortal.Domain.Services.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using FilingPortal.Parts.Common.Domain.Services.DB;

namespace FilingPortal.Domain.Tests.Services.Pipeline
{
    [TestClass]
    public class PipelineInboundFileValidationServiceTest : TestWithApplicationMapping
    {
        private PipelineInboundFileValidationService _service;

        private Mock<IParsingDataModelValidatorFactory> _validatorFactoryMock;
        private Mock<IParseModelMapRegistry> _modelMapRegistryMock;
        private Mock<ISqlQueryExecutor> _sqlQueryExecutorMock;

        private static PipelineInboundImportParsingModel CreateValidModel(Action<PipelineInboundImportParsingModel> action = null)
        {
            var model = new PipelineInboundImportParsingModel
            {
                Importer = "Importer",
                Batch = "Batch",
                TicketNumber = "TicketNumber",
                Quantity = 123456.12M,
                API = 12.12M,
                ExportDate = DateTime.Now,
                ImportDate = DateTime.Now,
                RowNumberInFile = 1,
                SiteName = "Site name",
                Facility = "Facility",
                EntryNumber = "EntryNumber"
            };
            action?.Invoke(model);
            return model;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _validatorFactoryMock = new Mock<IParsingDataModelValidatorFactory>();
            _validatorFactoryMock.Setup(x => x.Create<PipelineInboundImportParsingModel>()).Returns(new PipelineInboundParsingDataModelValidator());
            _modelMapRegistryMock = new Mock<IParseModelMapRegistry>();
            _modelMapRegistryMock.Setup(x => x.Get<PipelineInboundImportParsingModel>()).Returns(new TestModelMap());
            _sqlQueryExecutorMock = new Mock<ISqlQueryExecutor>();
            DataTable dataTable = GetDataTable();
            _sqlQueryExecutorMock.Setup(x => x.ExecuteSqlQuery(It.IsAny<string>())).Returns(dataTable);

            _service = new PipelineInboundFileValidationService(_validatorFactoryMock.Object, _modelMapRegistryMock.Object, _sqlQueryExecutorMock.Object);
        }

        private static DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("id");
            dataTable.Rows.Add(136);
            return dataTable;
        }

        [TestMethod]
        public void Validate_GetModelValidatorFromFactory()
        {
            _service.Validate(new[] { new PipelineInboundImportParsingModel() });
            _validatorFactoryMock.Verify(x => x.Create<PipelineInboundImportParsingModel>(), Times.Once);
        }

        [TestMethod]
        public void Validate_GetModelMapFromRegistry()
        {
            _service.Validate(new[] { new PipelineInboundImportParsingModel() });
            _modelMapRegistryMock.Verify(x => x.Get<PipelineInboundImportParsingModel>(), Times.Once);
        }

        [TestMethod]
        public void Validate_WithValidData_ReturnValidResult()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel() });
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WithValidData_ReturnResultWithValidData()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel(), CreateValidModel(x => x.Importer = "Importer 2") });
            Assert.AreEqual(2, result.ValidData.Count);
        }

        [TestMethod]
        public void Validate_WithValidAndInvalidData_ReturnResultWithValidData()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] {
                CreateValidModel(),
                CreateValidModel(x => x.Importer = "Importer 2"),
                CreateValidModel(x => x.Importer = string.Empty)
            });
            Assert.AreEqual(2, result.ValidData.Count);
        }

        [TestMethod]
        public void Validate_WithInvalidData_ResultNotValidResult()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { new PipelineInboundImportParsingModel() });
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_WithDublicatedData_ResultNotValidResult()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel(), CreateValidModel() });
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_WithInvalidData_ResultContainsErrorForEachInvalidColumn()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { new PipelineInboundImportParsingModel() });
            Assert.AreEqual(7, result.Errors.Count);
        }

        [TestMethod]
        public void Validate_WithTwoDuplicatedRecordsData_ResultContainsOneErrorMessage()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel(), CreateValidModel() });
            Assert.AreEqual(1, result.Errors.Count);
        }

        [TestMethod]
        public void Validate_WithTwoInvalidRecordsData_ResultContainsTwoErrorMessage()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel(x => x.Importer = string.Empty), CreateValidModel(x => x.Importer = string.Empty) });
            Assert.AreEqual(2, result.Errors.Count);
        }

        [TestMethod]
        public void Validate_WithOnDuplicatedRecordsInDB_ResultContainsOneErrorMessage()
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> result = _service.Validate(new[] { CreateValidModel(x => x.RowNumberInFile = 136) });
            Assert.AreEqual(1, result.Errors.Count);
        }

        private class TestModelMap : IParseModelMap
        {
            /// <summary>
            /// Get property name by column name
            /// </summary>
            /// <param name="columnName">Column name</param>
            public string GetPropertyName(string columnName)
            {
                throw new NotImplementedException();
            }

            public string SheetName => "SheetName";

            /// <summary>
            /// Gets the model type
            /// </summary>
            public Type GetModelType { get; }

            public string GetColumnName(string propertyName)
            {
                return propertyName;
            }

            /// <summary>
            /// Gets collection of the all <see cref="IPropertyMapInfo"/>
            /// </summary>
            public IEnumerable<IPropertyMapInfo> MapInfos { get; }

            public IPropertyMapInfo GetMapInfo(string modelFieldName)
            {
                return null;
            }
        }
    }
}

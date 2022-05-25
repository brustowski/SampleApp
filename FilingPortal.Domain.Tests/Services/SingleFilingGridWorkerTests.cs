using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class SingleFilingGridWorkerTests
    {
        public class TestBaseDefValuesManualReadModel : BaseDefValuesManualReadModel { };

        SingleFilingGridWorker<Entity, TestBaseDefValuesManualReadModel, BaseDocument> _service;
        private Mock<IDefValuesManualReadModelRepository<TestBaseDefValuesManualReadModel>> _repository;
        private Mock<IAgileConfiguration<Entity>> _agileGridRepository;
        private Mock<IDocumentRepository<BaseDocument>> _documentsRepository;
        private Mock<IDefValuesManualValidator<TestBaseDefValuesManualReadModel>> _validator;


        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new Mock<IDefValuesManualReadModelRepository<TestBaseDefValuesManualReadModel>>();
            _agileGridRepository = new Mock<IAgileConfiguration<Entity>>();
            _documentsRepository = new Mock<IDocumentRepository<BaseDocument>>();
            _validator = new Mock<IDefValuesManualValidator<TestBaseDefValuesManualReadModel>>();

            _service = new SingleFilingGridWorker<Entity, TestBaseDefValuesManualReadModel, BaseDocument>(_repository.Object, _agileGridRepository.Object, _documentsRepository.Object, _validator.Object);
        }

        [TestMethod]
        public void GetData_Calls_AgileGridRepository()
        {
            _service.GetData(new[] { 1 });
            _agileGridRepository.Verify(x => x.GetFields(), Times.Once);
        }

        [TestMethod]
        public void GetData_Calls_SingleFilingRepository()
        {
            _service.GetData(new[] { 1 });
            _repository.Verify(x => x.GetAllDataByFilingHeaderIds(It.IsAny<IEnumerable<int>>()), Times.Once);
        }

        [TestMethod]
        public void GetData_Calls_DocumentsRepository()
        {
            _service.GetData(new[] { 1 });
            _documentsRepository.Verify(x => x.GetDocumentsAmount(It.IsAny<IEnumerable<int>>()), Times.Once);
        }

        [TestMethod]
        public void GetData_Calls_Validation()
        {
            _service.GetData(new[] { 1 });
            _validator.Verify(x => x.ValidateDatabaseModels(It.IsAny<IEnumerable<TestBaseDefValuesManualReadModel>>()), Times.AtMostOnce);
        }

        [TestMethod]
        public void GetData_Sorts_By_FilingHeaderId()
        {
            var models = new List<TestBaseDefValuesManualReadModel>(5)
            {
                new TestBaseDefValuesManualReadModel{ FilingHeaderId = 1},
                new TestBaseDefValuesManualReadModel{ FilingHeaderId = 21},
                new TestBaseDefValuesManualReadModel{ FilingHeaderId = 4},
                new TestBaseDefValuesManualReadModel{ FilingHeaderId = 100},
                new TestBaseDefValuesManualReadModel{ FilingHeaderId = 0},
            };

            var documentsDict = new Dictionary<int, int>(5)
            {
                {1, 0},
                {21, 0},
                {4, 0},
                {100, 0},
                {0, 0}
            };

            var validationResult = new Dictionary<TestBaseDefValuesManualReadModel, DetailedValidationResult>();

            _repository.Setup(x => x.GetAllDataByFilingHeaderIds(It.IsAny<IEnumerable<int>>())).Returns(models);
            _documentsRepository.Setup(x => x.GetDocumentsAmount(It.IsAny<IEnumerable<int>>())).Returns(documentsDict);
            _validator.Setup(x => x.ValidateDatabaseModels(It.IsAny<IEnumerable<TestBaseDefValuesManualReadModel>>())).Returns(validationResult);

            var data = _service.GetData(null)
                .Select(x => x.Key)
                .ToArray();

            CollectionAssert.AreEqual(new[] { 0, 1, 4, 21, 100 }, data);
        }

        [TestMethod]
        public void GetTotalMatches_Calls_Repository()
        {
            var ids = new[] { 1 };
            _service.GetTotalMatches(ids);
            _repository.Verify(x => x.GetTotalMatches(ids), Times.Once);
        }

        private void InitDefaultMocksForGetData()
        {
            var validationResult = new Dictionary<TestBaseDefValuesManualReadModel, DetailedValidationResult>();

            _validator.Setup(x => x.ValidateDatabaseModels(It.IsAny<IEnumerable<TestBaseDefValuesManualReadModel>>())).Returns(validationResult);

            _documentsRepository.Setup(x => x.GetDocumentsAmount(It.IsAny<IEnumerable<int>>()))
                .Returns(new Dictionary<int, int> { { 1, 0 } });
        }

        [TestMethod]
        public void GetData_Doesnt_Duplicate_Records_With_Same_Record_Id()
        {
            // Assign
            var ids = new[] { 1 };
            var requiredField = new AgileField { TableName = "table1", ColumnName = "column", DisplayName = "Required column" };

            var configFields = new List<TestBaseDefValuesManualReadModel>
            {
                new TestBaseDefValuesManualReadModel {ColumnName = "column", TableName = "table1", RecordId = 1, Value = "FIELD_VALUE"},
                new TestBaseDefValuesManualReadModel {ColumnName = "column", TableName = "table1", RecordId = 1, Value = "FIELD_VALUE"}
            };

            InitDefaultMocksForGetData();

            _agileGridRepository.Setup(x => x.GetFields()).Returns(new List<AgileField> {requiredField});
            _repository.Setup(x => x.GetAllDataByFilingHeaderIds(ids)).Returns(configFields);
            
            // Act
            var data = _service.GetData(ids);
            // Assert

            Assert.IsTrue(data.Keys.Count == 1);
            var dynObject = data.Values.Single();
            var properties = dynObject.GetProperties();
            Assert.IsInstanceOfType(properties["column"], typeof(string));
            Assert.AreEqual("FIELD_VALUE", properties["column"]);
        }

        [TestMethod]
        public void GetData_Returns_Records_With_Same_Record_Id_And_Different_UniqueData()
        {
            // Assign
            var ids = new[] { 1 };
            var requiredField1 = new AgileField { TableName = "table1", ColumnName = "column", DisplayName = "Required column" };
            var requiredField2 = new AgileField { TableName = "table1", ColumnName = "column1", DisplayName = "Required column 1" };

            var configFields = new List<TestBaseDefValuesManualReadModel>
            {
                new TestBaseDefValuesManualReadModel {ColumnName = "column", TableName = "table1", RecordId = 1, Value = "FIELD_VALUE"},
                new TestBaseDefValuesManualReadModel {ColumnName = "column1", TableName = "table1", RecordId = 1, Value = "FIELD_VALUE"}
            };

            InitDefaultMocksForGetData();

            _agileGridRepository.Setup(x => x.GetFields()).Returns(new List<AgileField> { requiredField1, requiredField2 });
            _repository.Setup(x => x.GetAllDataByFilingHeaderIds(ids)).Returns(configFields);

            // Act
            var data = _service.GetData(ids);
            // Assert

            Assert.IsTrue(data.Keys.Count == 1);
            var dynObject = data.Values.Single();
            var properties = dynObject.GetProperties();
            Assert.IsInstanceOfType(properties["column"], typeof(string));
            Assert.IsInstanceOfType(properties["column1"], typeof(string));
            Assert.AreEqual("FIELD_VALUE", properties["column"]);
            Assert.AreEqual("FIELD_VALUE", properties["column1"]);
        }

        [TestMethod]
        public void GetData_Duplicate_Records_With_Different_Record_Id()
        {
            // Assign
            var ids = new[] { 1 };
            var requiredField = new AgileField { TableName = "table1", ColumnName = "column", DisplayName = "Required column" };

            var configFields = new List<TestBaseDefValuesManualReadModel>
            {
                new TestBaseDefValuesManualReadModel {ColumnName = "column", TableName = "table1", RecordId = 1, Value = "FIELD_VALUE"},
                new TestBaseDefValuesManualReadModel {ColumnName = "column", TableName = "table1", RecordId = 2, Value = "FIELD_VALUE"}
            };

            InitDefaultMocksForGetData();

            _agileGridRepository.Setup(x => x.GetFields()).Returns(new List<AgileField> { requiredField });
            _repository.Setup(x => x.GetAllDataByFilingHeaderIds(ids)).Returns(configFields);

            // Act
            var data = _service.GetData(ids);
            // Assert

            Assert.IsTrue(data.Keys.Count == 1);
            var dynObject = data.Values.Single();
            var properties = dynObject.GetProperties();
            Assert.IsInstanceOfType(properties["column"], typeof(string));
            Assert.AreEqual("FIELD_VALUE; FIELD_VALUE", properties["column"]);
        }
    }
}

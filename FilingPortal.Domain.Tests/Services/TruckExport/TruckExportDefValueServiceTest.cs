using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services.TruckExport
{
    [TestClass]
    public class TruckExportDefValueServiceTest
    {
        private IDefValueService<TruckExportDefValue> _service;
        private Mock<IDefValueRepository<TruckExportDefValue>> _defValuerepositoryMock;
        private Mock<ITablesRepository<TruckExportTable>> _tableRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _defValuerepositoryMock = new Mock<IDefValueRepository<TruckExportDefValue>>();
            _tableRepositoryMock = new Mock<ITablesRepository<TruckExportTable>>();

            _service = new DefValueService<TruckExportDefValue, TruckExportSection, TruckExportTable>(_defValuerepositoryMock.Object, _tableRepositoryMock.Object);
        }

        [TestMethod]
        public void Create_CalsTableRepository_WhenCalled()
        {
            var rule = new TruckExportDefValue();

            _service.Create(rule, "table_name", null);

            _tableRepositoryMock.Verify(x => x.GetSectionIdByTableName(It.Is<string>(v => v.Equals("table_name"))), Times.Once);
        }

        [TestMethod]
        public void Create_CalsDefValueRepository_WhenCalled()
        {
            var rule = new TruckExportDefValue();

            _service.Create(rule, "table_name", null);

            _defValuerepositoryMock.Verify(x => x.Add(It.IsAny<TruckExportDefValue>()), Times.Once);
            _defValuerepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Update_CalsTableRepository_WhenCalled()
        {
            var rule = new TruckExportDefValue
            {
                Id = 1
            };

            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            _service.Update(rule, "table_name", null);

            _tableRepositoryMock.Verify(x => x.GetSectionIdByTableName(It.Is<string>(v => v.Equals("table_name"))), Times.Once);
        }

        [TestMethod]
        public void Update_CalsDefValueRepository_WhenCalled()
        {
            var rule = new TruckExportDefValue
            {
                Id = 1
            };

            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            _service.Update(rule, "table_name", null);

            _defValuerepositoryMock.Verify(x => x.Update(It.IsAny<TruckExportDefValue>()), Times.Once);
            _defValuerepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Update_ReturnsOperationResult()
        {
            var rule = new TruckExportDefValue
            {
                Id = 1
            };

            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            var result = _service.Update(rule, "table_name", null);

            Assert.IsInstanceOfType(result, typeof(OperationResult));
        }

        [TestMethod]
        public void Update_Returns_ValidOperationResult()
        {
            var rule = new TruckExportDefValue
            {
                Id = 1
            };

            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            var result = _service.Update(rule, "table_name", null);

            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        public void Update_ReturnsInvalidOperationResult_ForNotExistingValue()
        {
            var rule = new TruckExportDefValue
            {
                Id = 1
            };

            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(false);

            var result = _service.Update(rule, "table_name", null);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        public void Delete_CalsDefValueRepository_WhenCalled()
        {
            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            _service.Delete(1);

            _defValuerepositoryMock.Verify(x => x.DeleteById(It.Is<int>(v => v == 1)), Times.Once);
            _defValuerepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Delete_ReturnsOperationResult()
        {
            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            var result = _service.Delete(1);

            Assert.IsInstanceOfType(result, typeof(OperationResult));
        }

        [TestMethod]
        public void Delete_Returns_ValidOperationResult()
        {
            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(true);

            var result = _service.Delete(1);

            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        public void Delete_ReturnsInvalidOperationResult_ForNotExistingValue()
        {
            _defValuerepositoryMock.Setup(x => x.IsExist(It.Is<int>(v => v == 1))).Returns(false);

            var result = _service.Delete(1);

            Assert.AreEqual(false, result.IsValid);
        }
    }
}
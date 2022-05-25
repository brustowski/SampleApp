using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers.Tests
{
    [TestClass()]
    public class VesselImportAddOrDeleteCommandHandlerTests
    {
        private VesselImportAddOrDeleteCommandHandler _handler;
        private Mock<IVesselImportReadModelRepository> _vesselImportReadModelRepository;
        private Mock<IVesselImportRepository> _vesselImportRepository;

        [TestInitialize]
        public void Init()
        {
            _vesselImportReadModelRepository = new Mock<IVesselImportReadModelRepository>();
            _vesselImportRepository = new Mock<IVesselImportRepository>();
            _handler = new VesselImportAddOrDeleteCommandHandler(_vesselImportReadModelRepository.Object, _vesselImportRepository.Object);
        }

        [TestMethod()]
        public void HandleTest_With_EmptyRecord_Returns_Failure()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().ErrorMessage, "Vessel import record can't be null");
        }

        [TestMethod()]
        public void HandleTest_Without_Record_Id_Adds_New_To_Repository()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();
            var import = new Mock<VesselImportRecord>();
            command.SetupGet(x => x.Record).Returns(import.Object);
            command.SetupGet(x => x.RecordId).Returns((int?)null);

            _vesselImportRepository
                .Setup(x => x.AddOrUpdate(import.Object))
                .Callback<VesselImportRecord>(x => x.Id = 10);

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            _vesselImportRepository.Verify(x => x.AddOrUpdate(import.Object), Times.AtMostOnce());
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(10, ((CommandResult<int>)result).Value);
        }

        [TestMethod()]
        public void HandleTest_With_Record_Id_Calls_ReadModel_Repository()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();
            var import = new Mock<VesselImportRecord>();
            command.SetupGet(x => x.Record).Returns(import.Object);
            command.SetupGet(x => x.RecordId).Returns(10);

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            _vesselImportReadModelRepository.Verify(x => x.Get(10), Times.AtMostOnce);
        }

        [TestMethod()]
        public void HandleTest_With_Record_Id_Cant_find_record()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();
            var import = new Mock<VesselImportRecord>();
            command.SetupGet(x => x.Record).Returns(import.Object);
            command.SetupGet(x => x.RecordId).Returns(10);

            _vesselImportReadModelRepository.Setup(x => x.Get(10)).Returns((VesselImportReadModel)null);

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().ErrorMessage, "Vessel import record with id = 10 not found");
        }

        [TestMethod()]
        public void HandleTest_With_Record_Id_Uneditable_Fails()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();
            var import = new Mock<VesselImportRecord>();
            command.SetupGet(x => x.Record).Returns(import.Object);
            command.SetupGet(x => x.RecordId).Returns(10);

            var readModelRecord = new Mock<VesselImportReadModel>();
            readModelRecord.Setup(x => x.CanEditInitialRecord()).Returns(false);

            _vesselImportReadModelRepository.Setup(x => x.Get(10)).Returns(readModelRecord.Object);

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.First().ErrorMessage, "Vessel import record can't be updated");
        }

        [TestMethod()]
        public void HandleTest_With_Record_Id_Editable_Edits_Record()
        {
            // Assign
            var command = new Mock<VesselImportAddOrUpdateCommand>();
            var import = new Mock<VesselImportRecord>();
            command.SetupGet(x => x.Record).Returns(import.Object);
            command.SetupGet(x => x.RecordId).Returns(10);

            var readModelRecord = new Mock<VesselImportReadModel>();
            readModelRecord.Setup(x => x.CanEditInitialRecord()).Returns(true);

            _vesselImportReadModelRepository.Setup(x => x.Get(10)).Returns(readModelRecord.Object);

            // Act
            CommandResult result = _handler.Handle(command.Object);

            // Assert
            _vesselImportRepository.Verify(x => x.AddOrUpdate(import.Object), Times.AtMostOnce());
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(10, ((CommandResult<int>)result).Value);
        }
    }
}
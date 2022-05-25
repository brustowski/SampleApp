using FilingPortal.Domain.Repositories.VesselImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace FilingPortal.Domain.Commands.Handlers.Tests
{
    [TestClass()]
    public class VesselImportMassDeleteCommandHandlerTests
    {
        private VesselImportMassDeleteCommandHandler _handler;
        private Mock<IVesselImportReadModelRepository> _vesselImportReadModelRepository;

        [TestInitialize]
        public void Init()
        {
            _vesselImportReadModelRepository = new Mock<IVesselImportReadModelRepository>();
            _handler = new VesselImportMassDeleteCommandHandler(_vesselImportReadModelRepository.Object);
        }

        [TestMethod()]
        public void HandleTest_Returns_Valid_Result_On_Empty_Ids_List()
        {
            // Assign
            var command = new Mock<VesselImportMassDeleteCommand>();
            command.SetupGet(x => x.RecordIds).Returns(new int[0]);

            // Act
            Framework.Domain.Commands.CommandResult result = _handler.Handle(command.Object);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [DataRow(1)]
        [DataRow(10)]
        [DataTestMethod]
        public void HandleTest_Calls_Repository_For_Each_Record(int amountOfRecordIds)
        {
            // Assign
            var command = new Mock<VesselImportMassDeleteCommand>();
            var recordIds = Enumerable.Repeat(10, amountOfRecordIds).ToArray();
            command.SetupGet(x => x.RecordIds).Returns(recordIds);

            // Act
            Framework.Domain.Commands.CommandResult result = _handler.Handle(command.Object);

            // Assert
            _vesselImportReadModelRepository.Verify(x => x.DeleteById(10), Times.Exactly(amountOfRecordIds));
        }

        [TestMethod]
        public void HandleTest_Cant_Delete_returns_Correct_Message()
        {
            // Assign
            var command = new Mock<VesselImportMassDeleteCommand>();
            var recordIds = new[] { 10 };
            command.SetupGet(x => x.RecordIds).Returns(recordIds);
            _vesselImportReadModelRepository.Setup(x => x.DeleteById(10)).Throws(new System.Exception());

            // Act
            Framework.Domain.Commands.CommandResult result = _handler.Handle(command.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unable to delete records with ids: 10", result.Errors.First().ErrorMessage);
        }

    }
}
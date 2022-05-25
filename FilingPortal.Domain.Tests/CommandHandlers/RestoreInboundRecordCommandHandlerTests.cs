using System;
using System.Linq;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Commands.Handlers;
using FilingPortal.Domain.Repositories.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.CommandHandlers
{   
    [TestClass]
    public class MassRestoreInboundRecordCommandHandlerTests
    {
        private RailInboundMassRestoreCommandHandler _handler;
        private Mock<IRailInboundReadModelRepository> _bdParsedRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _bdParsedRepositoryMock = new Mock<IRailInboundReadModelRepository>();

            _handler = new RailInboundMassRestoreCommandHandler(_bdParsedRepositoryMock.Object);
        }

        [TestMethod]
        public void Handle_ToDeleteRecord_CallsRepository()
        {
            var command = new RailInboundMassRestoreCommand { RecordIds = new[] { 21 } };

            _handler.Handle(command);

            _bdParsedRepositoryMock.Verify(x => x.RestoreById(21), Times.Once);
        }

        [TestMethod]
        public void Handle_DeleteRecord_ReturnsOk()
        {
            var command = new RailInboundMassRestoreCommand { RecordIds = new[] { 786 } };

            var result = _handler.Handle(command);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void Handle_WhenExceptionOccured_ReturnsFailure()
        {
            var command = new RailInboundMassRestoreCommand { RecordIds = new[] { 90 } };

            _bdParsedRepositoryMock.Setup(x => x.RestoreById(90)).Throws<Exception>();

            var result = _handler.Handle(command);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual($"Unable to restore records with ids: 90", result.Errors.ElementAt(0).ErrorMessage);
        }

    }
}

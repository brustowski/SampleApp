using FilingPortal.Domain.Common;
using FilingPortal.Domain.DocumentTypes;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class RailDocumentFactoryTests : TestWithApplicationMapping
    {
        private Mock<IDocumentTypesRepository> _documentTypeRepositoryMock;
        private RailDocumentFactory _factory;

        [TestInitialize]
        public void TestInitialize()
        {
            _documentTypeRepositoryMock = new Mock<IDocumentTypesRepository>();
            _factory = new RailDocumentFactory(_documentTypeRepositoryMock.Object);
        }

        [TestMethod]
        public void CreateFromDto_CreatesRailDocument_FromDtoWithFilledValues()
        {
            var fileContent = new byte[] { 1, 6, 7 };
            var dto = new RailDocumentDto
            {
                FileContent = fileContent,
                DocumentType = "doc type",
                FileDesc = "descr 1",
                FileExtension = "application/pdf",
                FileName = "some file",
                Status = InboundRecordDocumentStatus.New
            };

            RailDocument result = _factory.CreateFromDto(dto, null);

            Assert.AreEqual(fileContent.Length, result.Content.Length);
            Assert.AreEqual(dto.DocumentType, result.DocumentType);
            Assert.AreEqual(dto.FileDesc, result.Description);
            Assert.AreEqual(dto.FileName, result.FileName);
            Assert.AreEqual(dto.FileExtension, result.Extension);
            Assert.IsNotNull(result.CreatedDate);
            Assert.IsNull(result.CreatedUser);
        }

        [TestMethod]
        public void CreateFromDto_CreatesRailDocument_WithSpecifiedUser()
        {
            var fileContent = new byte[] { 1, 6, 7 };
            var dto = new RailDocumentDto
            {
                FileContent = fileContent,
                DocumentType = "doc type",
                FileDesc = "descr 1",
                FileExtension = "application/pdf",
                FileName = "some file",
                Status = InboundRecordDocumentStatus.New
            };

            RailDocument result = _factory.CreateFromDto(dto, "creator name");

            Assert.AreEqual("creator name", result.CreatedUser);
        }

        [TestMethod]
        public void CreateFromDto_CreatesRailDocumentManifest_FromModelWithFilledValues()
        {
            var fileContent = new byte[] { 1, 6, 7 };
            var fileModel = new BinaryFileModel
            {
                Content = fileContent,
                FileName = "some file.pdf"
            };

            RailDocument result = _factory.CreateManifest(fileModel, "creator");

            Assert.AreEqual(fileContent.Length, result.Content.Length);
            Assert.AreEqual("MAN", result.DocumentType);
            Assert.AreEqual(null, result.Description);
            Assert.AreEqual(fileModel.FileName, result.FileName);
            Assert.AreEqual(".pdf", result.Extension);
            Assert.IsNotNull(result.CreatedDate);
            Assert.AreEqual("creator", result.CreatedUser);
        }
    }
}

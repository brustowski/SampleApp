using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Documents.Truck
{
    [TestClass]
    public class TruckDocumentFactoryTests: TestWithApplicationMapping
    {
        private TruckDocumentFactory _factory;

        [TestInitialize]
        public void TestInitialize()
        {
            _factory = new TruckDocumentFactory();
        }

        [TestMethod]
        public void CreateFromDto_CreatesTruckDocument_FromDtoWithFilledValues()
        {
            var fileContent = new byte[] { 1, 6, 7 };
            var dto = new TruckDocumentDto
            {
                FileContent = fileContent,
                DocumentType = "doc type",
                FileDesc = "descr 1",
                FileExtension = "application/pdf",
                FileName = "some file",
                Status = InboundRecordDocumentStatus.New
            };

            var result = _factory.CreateFromDto(dto, null);

            Assert.AreEqual(fileContent.Length, result.Content.Length);
            Assert.AreEqual(dto.DocumentType, result.DocumentType);
            Assert.AreEqual(dto.FileDesc, result.Description);
            Assert.AreEqual(dto.FileName, result.FileName);
            Assert.AreEqual(dto.FileExtension, result.Extension);
            Assert.IsNotNull(result.CreatedDate);
            Assert.IsNull(result.CreatedUser);
        }

        [TestMethod]
        public void CreateFromDto_CreatesTruckDocument_WithSpecifiedUser()
        {
            var fileContent = new byte[] { 1, 6, 7 };
            var dto = new TruckDocumentDto
            {
                FileContent = fileContent,
                DocumentType = "doc type",
                FileDesc = "descr 1",
                FileExtension = "application/pdf",
                FileName = "some file",
                Status = InboundRecordDocumentStatus.New
            };

            var result = _factory.CreateFromDto(dto, "creator name");

            Assert.AreEqual("creator name", result.CreatedUser);
        }
    }
}

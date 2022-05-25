using System.Collections.Generic;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class FilingHeaderDocumentUpdateServiceTests
    {
        private FilingHeaderDocumentUpdateService<RailDocumentDto, RailDocument> _service;

        private Mock<IDocumentRepository<RailDocument>> _documentRepositoryMock;
        private Mock<IDocumentFactory<RailDocument>> _documentFactoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository<RailDocument>>();
            _documentFactoryMock = new Mock<IDocumentFactory<RailDocument>>();

            _service = new FilingHeaderDocumentUpdateService<RailDocumentDto, RailDocument>(_documentRepositoryMock.Object, _documentFactoryMock.Object);
        }

        [TestMethod]
        public void UpdateForFilingHeader_CallsFactory_ToAddNewDocuments()
        {
            int filingHeaderId = 12;
            RailDocumentDto newDto1 = new RailDocumentDto { FileName = "file name1", Status = InboundRecordDocumentStatus.New };
            RailDocumentDto newDto2 = new RailDocumentDto { FileName = "another name", Status = InboundRecordDocumentStatus.New };
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                newDto1,
                new RailDocumentDto {Id = 45, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted},
                newDto2

            };
            _documentFactoryMock.Setup((IDocumentFactory<RailDocument> x) => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentFactoryMock.Verify(x => x.CreateFromDto(newDto1, It.IsAny<string>()), Times.Once);
            _documentFactoryMock.Verify(x => x.CreateFromDto(newDto2, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateForFilingHeader_CallsRepository_ToAddNewDocuments()
        {
            int filingHeaderId = 12;
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                new RailDocumentDto {FileName = "file name1", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {FileName = "another name", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id=45,FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted}
            };
            _documentFactoryMock.Setup((IDocumentFactory<RailDocument> x) => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentRepositoryMock.Verify(x => x.Add(It.IsAny<RailDocument>()), Times.Exactly(2));
            _documentRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void UpdateForFilingHeader_CallsRepository_ToUpdateExistingDocuments()
        {
            int filingHeaderId = 12;
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                new RailDocumentDto {FileName = "file name1", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 96, FileName = "file name2", Status = InboundRecordDocumentStatus.Updated},
                new RailDocumentDto {FileName = "another name", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 45, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted}
            };
            _documentRepositoryMock.Setup(x => x.GetListByFilingHeader(12)).Returns(new List<RailDocument> { new RailDocument { Id = 96 } });
            _documentFactoryMock.Setup(x => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentRepositoryMock.Verify(x => x.GetListByFilingHeader(12), Times.Once);
            _documentRepositoryMock.Verify(x => x.Update(It.IsAny<RailDocument>()), Times.Once);
            _documentRepositoryMock.Verify(x => x.Update(It.Is<RailDocument>(d => d.Id == 96)), Times.Once);
            _documentRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void UpdateForFilingHeader_UpdatesDescriptionAndType_ForUpdatedDocuments()
        {
            int filingHeaderId = 12;
            byte[] oldFileContent = new byte[] { 1, 2, 3 };
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                new RailDocumentDto {FileName = "file name1", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto
                {
                    Id = 96,
                    FileName = "new file name",
                    FileDesc = "new description",
                    DocumentType = "new doc type",
                    FileContent = new byte[] {1, 7},
                    FileExtension = "new image/jpeg",
                    Status = InboundRecordDocumentStatus.Updated
                },
                new RailDocumentDto {FileName = "another name", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 45, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted}
            };
            _documentRepositoryMock.Setup(x => x.GetListByFilingHeader(12)).Returns(new List<RailDocument>
            {
                new RailDocument
                {
                    Id = 96,
                    FileName = "old file name",
                    Description = "old description",
                    DocumentType = "old type",
                    Extension = "old image/jpeg",
                    Content = oldFileContent
                }
            });
            _documentFactoryMock.Setup(x => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentRepositoryMock.Verify(x => x.Update(
                It.Is<RailDocument>(d => d.Id == 96 &&
                                         d.Description == "new description" &&
                                         d.DocumentType == "new doc type" &&
                                         d.FileName == "old file name" &&
                                         d.Extension == "old image/jpeg" &&
                                         d.Content == oldFileContent)), Times.Once);
            _documentRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void UpdateForFilingHeader_SkipsUpdatingDocument_IfNotExistsInRepository()
        {
            int filingHeaderId = 12;
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                new RailDocumentDto {FileName = "file name1", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 96, Status = InboundRecordDocumentStatus.Updated},
                new RailDocumentDto {FileName = "another name", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 45, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted}
            };
            _documentRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailDocument>());

            _documentFactoryMock.Setup((IDocumentFactory<RailDocument> x) => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentRepositoryMock.Verify(x => x.Update(It.IsAny<RailDocument>()), Times.Never);
        }

        [TestMethod]
        public void UpdateForFilingHeader_CallsRepository_ToDeleteDocuments()
        {
            int filingHeaderId = 12;
            List<RailDocumentDto> dtos = new List<RailDocumentDto>
            {
                new RailDocumentDto {FileName = "file name1", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {FileName = "another name", Status = InboundRecordDocumentStatus.New},
                new RailDocumentDto {Id = 45, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted},
                new RailDocumentDto {Id = 278, FileName = "file name3", Status = InboundRecordDocumentStatus.Deleted}
            };
            _documentFactoryMock.Setup((IDocumentFactory<RailDocument> x) => x.CreateFromDto(It.IsAny<RailDocumentDto>(), It.IsAny<string>())).Returns(new RailDocument());

            _service.UpdateForFilingHeader(filingHeaderId, dtos);

            _documentRepositoryMock.Verify(x => x.DeleteById(45), Times.Once);
            _documentRepositoryMock.Verify(x => x.DeleteById(278), Times.Once);
            _documentRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

    }
}

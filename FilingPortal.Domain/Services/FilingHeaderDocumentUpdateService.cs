using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service that updates filing header documents
    /// </summary>
    public class FilingHeaderDocumentUpdateService<TDocumentDto, TDocument> : IFilingHeaderDocumentUpdateService<TDocumentDto>
        where TDocument : BaseDocument
        where TDocumentDto : BaseDocumentDto
    {
        /// <summary>
        /// The Truck document repository
        /// </summary>
        protected readonly IDocumentRepository<TDocument> DocumentRepository;

        /// <summary>
        /// The Truck document factory
        /// </summary>
        protected readonly IDocumentFactory<TDocument> DocumentFactory;

        public FilingHeaderDocumentUpdateService(IDocumentRepository<TDocument> documentRepository, IDocumentFactory<TDocument> documentFactory)
        {
            DocumentRepository = documentRepository;
            DocumentFactory = documentFactory;
        }

        /// <summary>
        /// Updates existing documents of filing header with the specified identifier using the document DTO collection
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="dtos">The document DTO collection</param>
        /// <param name="ensureFilingHeader">Explicitly sets filing header id to documents</param>
        public void UpdateForFilingHeader(int filingHeaderId, IEnumerable<TDocumentDto> dtos, bool ensureFilingHeader = false)
        {
            var documentsDtoList = dtos.ToList();

            if (ensureFilingHeader) documentsDtoList.ForEach(x =>
            {
                x.FilingHeadersFk = filingHeaderId;
                if (x.Status == InboundRecordDocumentStatus.None)
                    x.Status = InboundRecordDocumentStatus.Updated;
            });

            var updatedDtos = documentsDtoList.Where(x => x.Status == InboundRecordDocumentStatus.Updated).ToList();
            var existingDocuments = DocumentRepository.GetListByFilingHeader(filingHeaderId).ToList();

            if (ensureFilingHeader)
                foreach (TDocument existingDocument in existingDocuments.Where(existingDocument => existingDocument.FilingHeaderId != filingHeaderId))
                {
                    existingDocument.FilingHeaderId = filingHeaderId;
                    DocumentRepository.Update(existingDocument);
                }

            foreach (TDocumentDto dto in documentsDtoList.Where(x => x.Status == InboundRecordDocumentStatus.New))
            {
                TDocument newDocument = DocumentFactory.CreateFromDto(dto, "sa");
                newDocument.FilingHeaderId = filingHeaderId;
                DocumentRepository.Add(newDocument);
            }
            foreach (TDocumentDto dto in updatedDtos)
            {
                TDocument existingDocument = existingDocuments.FirstOrDefault(x => x.Id == dto.Id);
                if (existingDocument == null)
                {
                    continue;
                }

                SetValuesToExistingDocument(dto, existingDocument);
                DocumentRepository.Update(existingDocument);
            }
            foreach (TDocumentDto document in documentsDtoList.Where(x => x.Status == InboundRecordDocumentStatus.Deleted))
            {
                DocumentRepository.DeleteById(document.Id);
            }
            DocumentRepository.Save();
        }

        /// <summary>
        /// Mass upload documents to inbound records
        /// </summary>
        /// <param name="inboundRecordIds">Inbound Records ids, where this document should be added</param>
        /// <param name="dtos">Document DTO collection</param>
        public virtual void UploadDocumentsToInboundRecords(int[] inboundRecordIds, IEnumerable<TDocumentDto> dtos)
        {
            var documentsDtoList = dtos.ToList();

            foreach (int inboundRecordId in inboundRecordIds)
            {
                var inboundDocsList = DocumentRepository.GetListByInboundRecord(inboundRecordId).Select(x =>
                    new
                    {
                        x.Id,
                        x.DocumentType
                    }).ToList();
                foreach (TDocumentDto dto in documentsDtoList.Where(x => x.Status == InboundRecordDocumentStatus.New))
                {
                    TDocument newDocument = DocumentFactory.CreateFromDto(dto, "sa");
                    newDocument.InboundRecordId = inboundRecordId;
                    DocumentRepository.AddOrUpdate(newDocument);

                    var sameDocType = inboundDocsList.FirstOrDefault(x => x.DocumentType == dto.DocumentType);
                    if (sameDocType != null)
                        DocumentRepository.DeleteById(sameDocType.Id);
                }
            }
            DocumentRepository.Save();
        }

        /// <summary>
        /// Sets the values from document DTO to existing document
        /// </summary>
        /// <param name="dto">The document DTO</param>
        /// <param name="existingDocument">The existing document</param>
        private void SetValuesToExistingDocument(TDocumentDto dto, BaseDocument existingDocument)
        {
            existingDocument.DocumentType = dto.DocumentType;
            existingDocument.Description = dto.FileDesc;
            existingDocument.FilingHeaderId = dto.FilingHeadersFk;
        }
    }
}

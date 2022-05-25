using System.Collections.Generic;
using FilingPortal.Domain.DTOs;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface for service that updates filing header documents
    /// </summary>
    public interface IFilingHeaderDocumentUpdateService<in TDocumentDto>
        where TDocumentDto : BaseDocumentDto
    {
        /// <summary>
        /// Updates existing documents of filing header with the specified identifier using the document DTO collection
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="dtos">The document DTO collection</param>
        /// <param name="ensureFilingHeader">Explicitly sets filing header id to documents</param>
        void UpdateForFilingHeader(int filingHeaderId, IEnumerable<TDocumentDto> dtos, bool ensureFilingHeader = false);

        /// <summary>
        /// Mass upload documents to inbound records
        /// </summary>
        /// <param name="inboundRecordIds">Inbound Records ids, where this document should be added</param>
        /// <param name="dtos">Document DTO collection</param>
        void UploadDocumentsToInboundRecords(int[] inboundRecordIds, IEnumerable<TDocumentDto> dtos);
    }
}
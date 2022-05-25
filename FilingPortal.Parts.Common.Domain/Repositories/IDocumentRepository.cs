using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of abstract document
    /// </summary>
    public interface IDocumentRepository<TDocument> : IRepository<TDocument>
        where TDocument : BaseDocument
    {
        /// <summary>
        /// Gets the list of document DTOs by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="inboundRecordsIds">Inbound Records ids</param>
        IEnumerable<TDocument> GetListByFilingHeader(int filingHeaderId, IEnumerable<int> inboundRecordsIds);
        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        IEnumerable<TDocument> GetListByFilingHeader(int filingHeaderId);
        /// <summary>
        /// Gets the list of document DTOs by inbound record identifier
        /// </summary>
        /// <param name="inboundRecordId">Inbound Records id</param>
        IEnumerable<TDocument> GetListByInboundRecord(int inboundRecordId);

        /// <summary>
        /// Returns documents amount for selected filing headers
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        IDictionary<int, int> GetDocumentsAmount(IEnumerable<int> filingHeaderIds);
    }
}
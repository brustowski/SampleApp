using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    /// <summary>
    /// Base class for documents repository
    /// </summary>
    /// <typeparam name="TDocument">Underlying document</typeparam>
    public abstract class BaseDocumentRepository<TDocument> : Repository<TDocument>, IDocumentRepository<TDocument>
        where TDocument : BaseDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDocumentRepository{TDocument}"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        protected BaseDocumentRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the list of document DTOs by filing header identifier
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifiers</param>
        protected IQueryable<IGrouping<int, TDocument>> GetDocumentsGroupedByFilingHeader(IEnumerable<int> filingHeaderIds)
        {
            return Set
                .Where(x => x.FilingHeaderId.HasValue && filingHeaderIds.Contains(x.FilingHeaderId.Value))
                .GroupBy(x => (int)x.FilingHeaderId);
        }

        /// <summary>
        /// Gets the list of document DTOs by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <param name="inboundRecordsIds">Inbound Records ids</param>
        public IEnumerable<TDocument> GetListByFilingHeader(int filingHeaderId, IEnumerable<int> inboundRecordsIds)
        {
            return Set.Where(x =>
                x.FilingHeaderId == filingHeaderId ||
                x.InboundRecordId.HasValue && inboundRecordsIds.Contains(x.InboundRecordId.Value)).ToList();
        }

        /// <summary>
        /// Gets the list of documents by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public abstract IEnumerable<TDocument> GetListByFilingHeader(int filingHeaderId);

        /// <summary>
        /// Gets the list of document DTOs by inbound record identifier
        /// </summary>
        /// <param name="inboundRecordId">Inbound Records id</param>
        public IEnumerable<TDocument> GetListByInboundRecord(int inboundRecordId)
        {
            return Set.Where(x =>
                x.InboundRecordId == inboundRecordId);
        }

        /// <summary>
        /// Returns documents amount for selected filing headers
        /// </summary>
        /// <param name="filingHeaderIds">List of filing headers</param>
        public IDictionary<int, int> GetDocumentsAmount(IEnumerable<int> filingHeaderIds)
        {
            var group = GetDocumentsGroupedByFilingHeader(filingHeaderIds)
                .Select(g => new { g.Key, Amount = g.Count() });
            return group.ToDictionary(x => x.Key, y => y.Amount);
        }
    }
}

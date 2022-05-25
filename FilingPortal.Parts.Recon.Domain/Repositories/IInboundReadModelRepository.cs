using System;
using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the inbound read models
    /// </summary>
    public interface IInboundReadModelRepository : ISearchRepository<InboundRecordReadModel>
    {
        /// <summary>
        /// Gets inbound records by entry numbers list
        /// </summary>
        /// <param name="entryNumbers">Entry numbers list</param>
        Task<IList<InboundRecordReadModel>> GetByEntryNumbers(IEnumerable<string> entryNumbers);
        /// <summary>
        /// Gets the <see cref="IEnumerable{InboundRecordReadModel}"/> filtered by expression
        /// </summary>
        /// <param name="filterExpression">Filtering expression</param>
        IEnumerable<InboundRecordReadModel> GetFiltered(Expression<Func<InboundRecordReadModel,bool>> filterExpression);
    }
}
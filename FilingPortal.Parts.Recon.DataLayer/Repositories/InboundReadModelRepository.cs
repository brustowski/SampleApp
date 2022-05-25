using AutoMapper.QueryableExtensions;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundRecord"/>
    /// </summary>
    public class InboundReadModelRepository : SearchRepository<InboundRecordReadModel>, IInboundReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundReadModelRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets inbound records by entry numbers list
        /// </summary>
        /// <param name="entryNumbers">Entry numbers list</param>
        public async Task<IList<InboundRecordReadModel>> GetByEntryNumbers(IEnumerable<string> entryNumbers)
        {
            return await Set.Where(x => entryNumbers.Contains(x.EntryNo)).ToListAsync();
        }

        /// <summary>
        /// Gets the <see cref="IEnumerable{InboundRecordReadModel}"/> filtered by expression
        /// </summary>
        /// <param name="filterExpression">Filtering expression</param>
        public IEnumerable<InboundRecordReadModel> GetFiltered(Expression<Func<InboundRecordReadModel, bool>> filterExpression)
        {
            return Set.Where(filterExpression).ToList();
        }
    }
}

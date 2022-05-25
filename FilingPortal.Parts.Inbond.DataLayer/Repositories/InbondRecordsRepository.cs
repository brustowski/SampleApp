using System.Collections.Generic;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundRecord"/>
    /// </summary>
    public class InbondRecordsRepository : SearchRepository<InboundRecord>, IInboundRecordsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondRecordsRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InbondRecordsRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork)
        {
        }
        
        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<InboundRecord> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }

        /// <summary>
        /// Checks if a record is present in the database
        /// </summary>
        /// <param name="record">The record to check</param>
        public bool IsDuplicated(InboundRecord record)
        {
            return false; // Duplicates are allowed
        }
    }
}

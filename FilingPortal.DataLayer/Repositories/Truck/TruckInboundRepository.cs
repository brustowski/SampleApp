using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using FilingPortal.Domain.Repositories.Truck;
    using Framework.DataLayer;

    /// <summary>
    /// Provides the repository of <see cref="TruckInbound"/>
    /// </summary>
    public class TruckInboundRepository : SearchRepository<TruckInbound>, ITruckInboundRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckInboundRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<TruckInbound> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }

        /// <summary>
        /// Checks whether specified record is not duplicating any other records
        /// </summary>
        /// <param name="record">The record to be checked</param>
        public bool IsDuplicate(TruckInbound record)
        {
            return record != null && Set.Any(x=> x.SupplierCode == record.SupplierCode && x.ImporterCode == record.ImporterCode && x.PAPs == record.PAPs);
        }
    }
}

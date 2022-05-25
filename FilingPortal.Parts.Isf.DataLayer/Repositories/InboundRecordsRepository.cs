using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundRecord"/>
    /// </summary>
    public class InboundRecordsRepository : SearchRepository<InboundRecord>, IInboundRecordsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundRecordsRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
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
            return Set.Any(x => x.Deleted == false
                && x.Id != record.Id
                && x.ImporterId.Equals(record.ImporterId)
                && x.BuyerId == record.BuyerId
                && x.ConsigneeId == record.ConsigneeId
                && x.MblScacCode.Equals(record.MblScacCode)
                && x.Eta == record.Eta
                && x.Etd == record.Etd
                && x.OwnerRef == record.OwnerRef
                && x.SellerId == record.SellerId
                && x.ShipToId == record.ShipToId
                && x.ContainerStuffingLocationId == record.ContainerStuffingLocationId
                && x.ConsolidatorId == record.ConsolidatorId
                );
        }
    }
}

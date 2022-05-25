using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
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
                && x.VendorId.Equals(record.VendorId)
                && x.Port.Equals(record.Port)
                && x.ParsNumber.Equals(record.ParsNumber)
                && x.Eta == record.Eta
                && x.OwnersReference.Equals(record.OwnersReference)
                && x.GrossWeight == record.GrossWeight
                && x.DirectShipDate == record.DirectShipDate
                && x.ConsigneeId == record.ConsigneeId
                && x.ProductCodeId == record.ProductCodeId
                && x.InvoiceQty == record.InvoiceQty
                && x.LinePrice == record.LinePrice
                );
        }
    }
}

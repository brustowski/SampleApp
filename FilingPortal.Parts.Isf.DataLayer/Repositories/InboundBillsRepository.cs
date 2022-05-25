using System.Linq;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Represents repository of the <see cref="InboundBillRecord"/>
    /// </summary>
    internal class InboundBillsRepository : SearchRepository<InboundBillRecord>, IBillsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundBillRecord"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundBillsRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Removes all Bills by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        public void RemoveBills(InboundRecord record) => RemoveBills(record.Id);

        /// <summary>
        /// Removes all Bills by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        public void RemoveBills(int inboundRecordId)
        {
            IQueryable<InboundBillRecord> bills = Set.Where(x => x.InboundRecordId == inboundRecordId);
            Set.RemoveRange(bills);
        }

        /// <summary>
        /// Checks if bill number exists
        /// </summary>
        /// <param name="record">Bill record</param>
        public bool Exists(InboundBillRecord record)
        {
            return Set.Any(x =>
                x.Inbound != null 
                && !x.Inbound.Deleted 
                && x.BillNumber == record.BillNumber 
                && x.InboundRecordId != record.InboundRecordId);
        }
    }
}
using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="InboundBillRecord"/>
    /// </summary>
    public interface IBillsRepository : ISearchRepository<InboundBillRecord>
    {
        /// <summary>
        /// Removes all bills by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        void RemoveBills(InboundRecord record);

        /// <summary>
        /// Removes all bills by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        void RemoveBills(int inboundRecordId);

        /// <summary>
        /// Checks if bill number exists
        /// </summary>
        /// <param name="record">Bill record</param>
        bool Exists(InboundBillRecord record);
    }
}
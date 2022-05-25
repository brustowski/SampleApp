using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="InboundManufacturerRecord"/>
    /// </summary>
    public interface IManufacturersRepository : ISearchRepository<InboundManufacturerRecord>
    {
        /// <summary>
        /// Removes all manufacturers by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        void RemoveManufacturers(InboundRecord record);

        /// <summary>
        /// Removes all manufacturers by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        void RemoveManufacturers(int inboundRecordId);
    }
}
using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="InboundContainerRecord"/>
    /// </summary>
    public interface IContainersRepository : ISearchRepository<InboundContainerRecord>
    {
        /// <summary>
        /// Removes all containers by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        void RemoveContainers(InboundRecord record);

        /// <summary>
        /// Removes all containers by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        void RemoveContainers(int inboundRecordId);
    }
}
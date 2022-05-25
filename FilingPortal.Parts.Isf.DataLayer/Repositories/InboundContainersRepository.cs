using System.Linq;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Represents repository of the <see cref="InboundContainerRecord"/>
    /// </summary>
    internal class InboundContainersRepository : SearchRepository<InboundContainerRecord>, IContainersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundContainerRecord"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundContainersRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Removes all Containers by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        public void RemoveContainers(InboundRecord record) => RemoveContainers(record.Id);

        /// <summary>
        /// Removes all Containers by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        public void RemoveContainers(int inboundRecordId)
        {
            IQueryable<InboundContainerRecord> containers = Set.Where(x => x.InboundRecordId == inboundRecordId);
            Set.RemoveRange(containers);
        }
    }
}
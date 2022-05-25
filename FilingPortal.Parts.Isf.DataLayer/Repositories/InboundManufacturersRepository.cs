using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Represents repository of the <see cref="InboundManufacturerRecord"/>
    /// </summary>
    internal class InboundManufacturersRepository : SearchRepository<InboundManufacturerRecord>, IManufacturersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundManufacturerRecord"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundManufacturersRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Removes all manufacturers by inbound record
        /// </summary>
        /// <param name="record">Inbound record</param>
        public void RemoveManufacturers(InboundRecord record) => RemoveManufacturers(record.Id);

        /// <summary>
        /// Removes all manufacturers by inbound record id
        /// </summary>
        /// <param name="inboundRecordId">Inbound record id</param>
        public void RemoveManufacturers(int inboundRecordId)
        {
            IQueryable<InboundManufacturerRecord> manufacturers = Set.Where(x => x.InboundRecordId == inboundRecordId);
            if (manufacturers.Any())
            {
                IQueryable<AppAddress> addressesToRemove = manufacturers
                    .Where(x => x.ManufacturerAppAddress != null)
                    .Select(x => x.ManufacturerAppAddress);
                UnitOfWork.Context.Set<AppAddress>().RemoveRange(addressesToRemove);

                Set.RemoveRange(manufacturers);
            }
        }
    }
}
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
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
            return Set.Include(x => x.Containers)
                .Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }

        /// <summary>
        /// Checks if a record is present in the database
        /// </summary>
        /// <param name="record">The record to check</param>
        public bool IsDuplicated(InboundRecord record)
        {
            var containersNumbers = record.Containers.Select(x => x.ContainerNumber);
            return Set.Include(x=>x.Containers).Any(x =>
                x.Exporter == record.Exporter &&
                x.Importer == record.Importer &&
                x.MasterBill == record.MasterBill &&
                x.LoadPort == record.LoadPort &&
                x.Carrier == record.Carrier &&
                x.TariffType == record.TariffType &&
                x.Tariff == record.Tariff &&
                x.GoodsDescription == record.GoodsDescription &&
                x.GrossWeightUOM == record.GrossWeightUOM &&
                x.Containers.Any(c=>containersNumbers.Contains(c.ContainerNumber)) &&
                x.Deleted == false
                );
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using Framework.DataLayer;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundReadModel"/>
    /// </summary>
    public class InboundReadModelRepository : SearchRepository<InboundReadModel>, IInboundReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InboundReadModelRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<InboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            List<InboundReadModel> records = GetSet<FilingHeader>().Where(x => filingHeaderIds.Contains(x.Id))
                .ToList()
                .SelectMany(x => x.InboundRecords.Select(y => new InboundReadModel
                {
                    Id = y.Id,
                    FilingHeaderId = x.Id,
                    MappingStatus = x.MappingStatus,
                    FilingStatus = x.FilingStatus,
                    FilingNumber = x.FilingNumber,
                    Exporter = y.Exporter,
                    Importer = y.Importer,
                    TariffType = y.TariffType,
                    Tariff = y.Tariff,
                    MasterBill = y.MasterBill,
                    GoodsDescription = y.GoodsDescription,
                    GrossWeightUOM = y.GrossWeightUOM,
                    CreatedDate = y.CreatedDate,
                    Confirmed = x.Confirmed,
                })).ToList();
            return records;
        }

        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId) =>
            new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };

        /// <summary>
        /// Soft Delete of the record with specified identifier
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        public override void DeleteById(int id)
        {
            InboundRecord record = UnitOfWork.Context.Set<InboundRecord>().Find(id);
            if (record != null)
            {
                record.Deleted = true;
                UnitOfWork.Context.SaveChanges();
            }
        }
    }
}

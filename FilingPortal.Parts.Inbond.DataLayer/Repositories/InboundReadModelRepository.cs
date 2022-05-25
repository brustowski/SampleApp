using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using Framework.DataLayer;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
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
        public InboundReadModelRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Soft Delete of the record with specified identifier
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        public override void DeleteById(int id)
        {
            var record = UnitOfWork.Context.Set<InboundRecord>().Find(id);
            if (record != null)
            {
                record.Deleted = true;
                UnitOfWork.Context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId) =>
            new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };

        /// <summary>
        /// Gets records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<InboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            List<InboundReadModel> records = GetSet<FilingHeader>().Where(x => filingHeaderIds.Contains(x.Id))
                .ToList()
                .SelectMany(x => x.InbondRecords.Select(y => new InboundReadModel
                {
                    Id = y.Id,
                    FilingHeaderId = x.Id,
                    MappingStatus = x.MappingStatus,
                    FilingStatus = x.FilingStatus,
                    CreatedDate = y.CreatedDate,
                    CreatedUser = y.CreatedUser,
                    FilingNumber = x.FilingNumber,
                    IsDeleted = y.Deleted,
                    JobLink = x.JobLink,
                    FirmsCode = y.FirmsCode.FirmsCode,
                    ImporterCode = y.Importer?.ClientCode,
                    PortOfArrival = y.PortOfArrival,
                    ExportConveyance = y.ExportConveyance,
                    ConsigneeCode = y.Consignee?.ClientCode,
                    Value = y.Value,
                    ManifestQty = y.ManifestQty,
                    ManifestQtyUnit = y.ManifestQtyUnit,
                    Weight = y.Weight,
                })).ToList();
            return records;
        }
    }
}

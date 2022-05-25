using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Repositories;
using Framework.DataLayer;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
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
                    ImporterCode = y.Importer?.ClientCode,
                    ConsigneeCode = y.Consignee?.ClientCode,
                    OwnerRef = y.OwnerRef,
                    Eta = y.Eta,
                    FilingNumber = x.FilingNumber,
                    IsDeleted = y.Deleted,
                    JobLink = x.JobLink,
                    CreatedDate = y.CreatedDate,
                    MblScacCode = y.MblScacCode,
                    ContainerStuffingLocationCode = y.ContainerStuffingLocation?.ClientCode,
                    ConsolidatorCode = y.Consolidator?.ClientCode,
                    ShipToCode = y.ShipTo?.ClientCode,
                    SellerCode = y.Seller?.ClientCode,
                    Etd = y.Etd,
                    BuyerCode = y.Buyer?.ClientCode,
                })).ToList();
            return records;
        }

        /// <summary>
        /// Gets created users list
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        public IList<string> GetUsers(SearchRequest searchRequest)
        {
            var query = Set.OrderByField(searchRequest.SortingSettings);
            if (searchRequest.Specification != null)
            {
                var specification = (ISpecification<InboundReadModel>)searchRequest.Specification;
                query = query.Where(specification.GetExpression());
            }

            return query.Select(x => x.CreatedUser).Distinct().ToList();
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

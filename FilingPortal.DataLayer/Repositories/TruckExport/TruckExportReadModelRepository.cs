using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.DataLayer;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Defines repository for the Truck Export Read Model
    /// </summary>
    public class TruckExportReadModelRepository : SearchRepository<TruckExportReadModel>, ITruckExportReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        /// Mass mark as delete action
        /// </summary>
        /// <param name="ids">Ids of records to mark as delete</param>
        public override void Delete(IEnumerable<int> ids) => ids.ForEach(DeleteById);

        /// <summary>
        /// Mark the record as deleted
        /// </summary>
        /// <param name="id">The record identifier</param>
        public override void DeleteById(int id)
        {
            SetDeleteStatus(id, true);
        }


        /// <summary>
        /// Calls procedure for soft-delete record with the specified identifier by setting Deleted flag
        /// </summary>
        /// <param name="id">The Rail BD Parsed record identifier</param>
        /// <param name="isDeleted">Deletion flag</param>
        private int SetDeleteStatus(int id, bool isDeleted)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Input,
                Value = id
            };
            var isDeletedParam = new SqlParameter
            {
                ParameterName = "@deleted",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Input,
                Value = isDeleted
            };

            const string command = "EXEC dbo.sp_exp_truck_delete_inbound @id, @deleted";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParam, isDeletedParam);

            return (int)idParam.Value;
        }


        /// <summary>
        /// Gets Truck Export records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        public IEnumerable<TruckExportReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds)
        {
            var records = GetSet<TruckExportFilingHeader>().Where(x => filingHeaderIds.Contains(x.Id))
                .ToList()
                .SelectMany(x => x.TruckExports.Select(y => new TruckExportReadModel
                {
                    Id = y.Id,
                    FilingHeaderId = x.Id,
                    JobStatus = x.JobStatus,
                    FilingNumber = x.FilingNumber,
                    Exporter = y.Exporter,
                    Importer = y.Importer,
                    TariffType = y.TariffType,
                    Tariff = y.Tariff,
                    RoutedTran = y.RoutedTran,
                    SoldEnRoute = y.SoldEnRoute,
                    MasterBill = y.MasterBill,
                    Origin = y.Origin,
                    Export = y.Export,
                    ExportDate = y.ExportDate,
                    ECCN = y.ECCN,
                    GoodsDescription = y.GoodsDescription,
                    CustomsQty = y.CustomsQty,
                    Price = y.Price,
                    GrossWeight = y.GrossWeight,
                    GrossWeightUOM = y.GrossWeightUOM,
                    Hazardous = y.Hazardous,
                    CreatedDate = y.CreatedDate,
                })).ToList();
            return records;
        }

        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId)
        {
            return new TableInfo { RowCount = Set.Count(x => x.FilingHeaderId == filingHeaderId) };
        }

        /// <summary>
        /// Gets the collection of the users specified by search request
        /// </summary>
        /// <param name="request">The search request</param>
        public IEnumerable<string> GetUsers(SearchRequest request)
        {
            IQueryable<TruckExportReadModel> query = Set.OrderByField(request.SortingSettings);
            if (request.Specification != null)
            {
                var specification = (ISpecification<TruckExportReadModel>)request.Specification;
                query = query.Where(specification.GetExpression());
            }

            string[] result = query.Select(x => x.ModifiedUser).Distinct().ToArray();
            return result;
        }
    }
}
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.DataLayer;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{

    /// <summary>
    /// Defines the repository of the Truck Export
    /// </summary>
    public class TruckExportRepository : SearchRepository<TruckExportRecord>, ITruckExportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<TruckExportRecord> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }

        /// <summary>
        /// Gets the matched entity form the repository for the provided entity or null if entity not found
        /// </summary>
        /// <param name="entity">The entity to search</param>
        public TruckExportRecord GetMatchedEntity(TruckExportImportModel entity)
        {
            return Set.FirstOrDefault(x => !x.Deleted && x.Exporter == entity.Exporter && x.MasterBill == entity.MasterBill);
        }

        /// <summary>
        /// Gets all records with AutoRefile set to true
        /// </summary>
        public IEnumerable<TruckExportRecord> GetAutoRefileRecords()
        {
            return Set.Where(x => x.IsAuto && !x.IsAutoProcessed).ToList();
        }

        /// <summary>
        /// Gets the collection of the users specified by search request
        /// </summary>
        /// <param name="request">The search request</param>
        public IList<string> GetUsers(SearchRequest request)
        {
            IQueryable<TruckExportRecord> query = Set.OrderByField(request.SortingSettings);
            if (request.Specification != null)
            {
                var specification = (ISpecification<TruckExportRecord>)request.Specification;
                query = query.Where(specification.GetExpression());
            }

            var result = query.Select(x => x.CreatedUser).Distinct().ToList();
            return result;
        }

        /// <summary>
        /// Runs Database validation of truck export records
        /// </summary>
        /// <param name="records"></param>
        public void Validate(IList<TruckExportRecord> records)
        {
            Validate(records.Select(x => x.Id));

            foreach (TruckExportRecord truckExportRecord in records)
            {
                UnitOfWork.Context.Entry(truckExportRecord).Reload();
            }
        }

        /// <summary>
        /// Runs Database validation of truck export records
        /// </summary>
        /// <param name="recordIds">Truck Export records ids</param>
        public void Validate(IEnumerable<int> recordIds)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@ids",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = string.Join(",", recordIds)
            };

            const string command = "EXEC dbo.sp_exp_truck_validate @ids";
            var context = (FilingPortalContext)UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParam);
        }

        /// <summary>
        /// Try to get the job number for specified record
        /// </summary>
        /// <param name="record">Inbound record</param>
        public string TryGetJobNumber(TruckExportRecord record)
        {
            var masterBillParam = new SqlParameter
            {
                ParameterName = "@masterBill",
                SqlDbType = SqlDbType.VarChar,
                Size = 35,
                Direction = ParameterDirection.Input,
                Value = record.MasterBill
            };

            var resultParam = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.VarChar,
                Size = 50,
                Direction = ParameterDirection.Output,
            };

            var command = $"EXEC {UnitOfWork.Context.DefaultSchema}.sp_exp_truck_get_job_number @masterBill, @result OUTPUT";

            DbExtendedContext context = UnitOfWork.Context;
            context.Database.ExecuteSqlCommand(command, masterBillParam, resultParam);

            return resultParam.Value is string jobNumber ? jobNumber : null;
        }
    }
}

using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using Framework.DataLayer;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Repositories
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
            var records = GetSet<FilingHeader>().Where(x => filingHeaderIds.Contains(x.Id))
                //.Include("InboundRecords.Importer")
                .Include(x => x.InboundRecords.Select(y => y.Applicant))
                .Include(x => x.InboundRecords.Select(y => y.FtzOperator))
                .ToList()
                .SelectMany(x => x.InboundRecords.Select(y => new InboundReadModel
                {
                    Id = y.Id,
                    FilingHeaderId = x.Id,
                    JobStatus = x.JobStatus,
                    FilingNumber = x.FilingNumber,
                    CreatedDate = y.CreatedDate,
                    Applicant = y.Applicant.ClientCode,
                    // Importer = y.Applicant.ClientCode,
                    AdmissionType = y.AdmissionType,
                    Ein = y.Ein,
                    ModifiedDate = y.ModifiedDate,
                    ModifiedUser = y.ModifiedUser,
                    IsAuto = y.IsAuto,
                    ZoneId = y.ZoneId,
                    FtzOperator = y.FtzOperator.ClientCode,
                    //Importer = y.Importer?.ClientCode,
                    //OwnerRef = y.OwnerRef,
                    //FirmsCode = y.FirmsCode,
                    //ArrivalDate = y.ArrivalDate,
                    //EntryPort = y.EntryPort
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

            var context = (PluginContext)UnitOfWork.Context;

            string command = $"EXEC {context.DefaultSchema}.sp_inbound_validate @ids";
            
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, command, idParam);
        }
    }
}

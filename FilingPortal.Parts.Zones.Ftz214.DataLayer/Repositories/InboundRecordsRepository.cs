using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="InboundRecord"/>
    /// </summary>
    public class InboundRecordsRepository : SearchRepository<InboundRecord>, IInboundRecordsRepository, IAutofileMethodsRepository<InboundRecord>, IValidationRepository<InboundRecord>
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
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }

        /// <summary>
        /// Checks if a record is present in the database
        /// </summary>
        /// <param name="record">The record to check</param>
        public bool IsDuplicated(InboundRecord record)
        {
            return Set.Where(rec => rec.InboundParsedData.AdmissionNo == record.InboundParsedData.AdmissionNo
            && rec.InboundParsedData.AdmissionYear == record.InboundParsedData.AdmissionYear
            && rec.InboundParsedData.ZoneNo == record.InboundParsedData.ZoneNo
            && rec.Deleted == false).Count() > 0;
        }

        /// <summary>
        /// Gets matched entity from repository
        /// </summary>
        /// <param name="item">Imported entry</param>
        public InboundRecord GetMatchedEntity(FTZ_214FTZ_ADMISSION item)
        {
            return Set.SingleOrDefault(rec => rec.InboundParsedData.AdmissionNo.ToLower() == item.ADMISSION_NO.ToLower()
            && rec.InboundParsedData.AdmissionYear.ToLower() == item.ADMISSION_YEAR.ToLower()
            && rec.InboundParsedData.ZoneNo.ToLower() == item.ZONE_NO.ToLower()
            && rec.Deleted == false);
        }

        /// <summary>
        /// Removes inbound lines from record
        /// </summary>
        /// <param name="item">Record</param>
        public void ClearParsedLines(InboundRecord item)
        {
            GetSet<InboundParsedLine>().RemoveRange(item.InboundParsedLines);
        }

        /// <summary>
        /// Removes parsed data from record
        /// </summary>
        /// <param name="item">Record</param>
        public void ClearParsedData(InboundRecord item)
        {
            if (item.InboundParsedData != null)
                GetSet<InboundParsedData>().Remove(item.InboundParsedData);
        }

        /// <summary>
        /// Gets all records with AutoRefile set to true
        /// </summary>
        public IEnumerable<InboundRecord> GetAutoRefileRecords()
        {
            return Set.Where(x => x.IsAuto && !x.IsAutoProcessed).ToList();
        }

        /// <summary>
        /// Runs Database validation on records
        /// </summary>
        /// <param name="records">Records</param>
        public void Validate(IList<InboundRecord> records)
        {
            Validate(records.Select(x => x.Id));

            foreach (InboundRecord record in records)
            {
                UnitOfWork.Context.Entry(record).Reload();
            }
        }

        /// <summary>
        /// Runs Database validation of inbound records
        /// </summary>
        /// <param name="recordIds">Records ids</param>
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

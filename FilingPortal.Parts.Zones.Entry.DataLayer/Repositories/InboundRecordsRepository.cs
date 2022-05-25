using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
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
            return Set.Any(x =>
                  x.EntryNo == record.EntryNo &&
                  x.Deleted == false
                );
        }

        /// <summary>
        /// Gets matched entity from repository
        /// </summary>
        /// <param name="item">Imported entry</param>
        public InboundRecord GetMatchedEntity(CUSTOMS_ENTRY_FILEENTRY item)
        {
            var entryNo = $"{item.ENTRY_NO}{item.CHECK_DIGIT}";
            return Set.FirstOrDefault(x => !x.Deleted && x.EntryNo == entryNo && x.FilerCode == item.FILER_CODE);
        }

        /// <summary>
        /// Removes inbound lines from record
        /// </summary>
        /// <param name="item">Record</param>
        public void ClearLines(InboundRecord item)
        {
            GetSet<InboundLine>().RemoveRange(item.InboundLines);
        }

        /// <summary>
        /// Removes inbound notes from record
        /// </summary>
        /// <param name="item">Record</param>
        public void ClearNotes(InboundRecord item)
        {
            GetSet<InboundNote>().RemoveRange(item.Notes);
        }

        /// <summary>
        /// Removes parsed data from record
        /// </summary>
        /// <param name="item">Record</param>
        public void ClearParsedData(InboundRecord item)
        {
            if (item.ParsedData != null)
                GetSet<InboundParsedData>().Remove(item.ParsedData);
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

using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;
using Framework.DataLayer.Extensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="AceReportRecord"/>
    /// </summary>
    public class AceReportRecordsRepository : SearchRepository<AceReportRecord>, IRuleRepository<AceReportRecord>, IAceReportRecordsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportRecordsRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public AceReportRecordsRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(AceReportRecord rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(AceReportRecord rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id
                                  && x.EntrySummaryNumber == rule.EntrySummaryNumber
                                  && x.EntrySummaryLineNumber == rule.EntrySummaryLineNumber)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);



        /// <summary>
        /// Gets inbound records by entry numbers list
        /// </summary>
        /// <param name="entryNumbers">Entry numbers list</param>
        public async Task<IList<AceReportRecord>> GetByEntryNumbers(IEnumerable<string> entryNumbers)
        {
            return await Set.Where(x => entryNumbers.Contains(x.EntrySummaryNumber)).ToListAsync();
        }

        /// <summary>
        /// Clears the ACE Report table
        /// </summary>
        public void Clear()
        {
            var tableName = UnitOfWork.Context.GetTableName<AceReportRecord>();
            UnitOfWork.Context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, $"DELETE FROM {tableName}");
        }

        /// <summary>
        /// Checks if the repository contains any records
        /// </summary>
        public bool IsEmpty()
        {
            return Set.Any();
        }

        /// <summary>
        /// Gets missing in CW report records 
        /// </summary>
        public async Task<IEnumerable<AceReportRecord>> GetMissingRecords()
        {
            IQueryable<InboundRecordReadModel> cwSet = GetSet<InboundRecordReadModel>().AsQueryable();
            return await Set.Where(x => !cwSet.Any(y => y.AceId == x.Id)).ToListAsync();
        }
    }
}

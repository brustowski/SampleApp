using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Repositories.Audit.Rail;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Audit.Rail
{
    /// <summary>
    /// Repository of the <see cref="AuditRailDailySpiRule"/> entity
    /// </summary>
    public class RailDailyAuditSpiRulesRepository : SearchRepositoryWithTypedId<AuditRailDailySpiRule, int>, IRailDailyAuditSpiRulesRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="RailDailyAuditSpiRulesRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public RailDailyAuditSpiRulesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The rule to be checked</param>
        public bool IsDuplicate(AuditRailDailySpiRule rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(AuditRailDailySpiRule rule)
        {
            return Set.Where(x => x.Id != rule.Id
                                           && x.ImporterCode == rule.ImporterCode
                                           && x.SupplierCode == rule.SupplierCode
                                           && x.GoodsDescription == rule.GoodsDescription
                                           && x.DestinationState == rule.DestinationState
                                           && x.CustomsAttrib4 == rule.CustomsAttrib4
                                           && ((rule.DateFrom >= x.DateFrom && rule.DateFrom <= x.DateTo) ||
                                               (rule.DateTo >= x.DateFrom && rule.DateTo <= x.DateTo) ||
                                               (rule.DateFrom >= x.DateFrom && rule.DateTo <= x.DateTo)))
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);

        /// <summary>
        /// Finds spi rules to process current record
        /// </summary>
        /// <param name="record">Rail daily audit record</param>
        public async Task<IList<AuditRailDailySpiRule>> FindCorrespondingRules(AuditRailDaily record)
        {
            if (!record.ReleaseDate.HasValue || string.IsNullOrWhiteSpace(record.GoodsDescription))
                return new List<AuditRailDailySpiRule>();

            List<AuditRailDailySpiRule> rules = await Set.Where(x =>
                x.ImporterCode == record.ImporterCode
                && (record.SupplierCode == x.SupplierCode || x.SupplierCode == null)
                && record.GoodsDescription.Contains(x.GoodsDescription)
                && (record.DestinationState == x.DestinationState || x.DestinationState == null)
                && (record.CustomsAttrib4 == x.CustomsAttrib4 || x.CustomsAttrib4 == null)
                && record.ReleaseDate.Value >= x.DateFrom
                && record.ReleaseDate.Value <= x.DateTo
                ).ToListAsync();

            List<AuditRailDailySpiRule> result = new List<AuditRailDailySpiRule>();
            int resultWeight = 0;

            foreach (AuditRailDailySpiRule rule in rules)
            {
                int weight = GetWeight(rule, record);
                if (weight > resultWeight)
                {
                    result = new List<AuditRailDailySpiRule> { rule };
                    resultWeight = weight;
                }
                else
                if (weight == resultWeight)
                    result.Add(rule);
            }

            return result;
        }

        /// <summary>
        /// Counts rule weight
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="record">Rail daily audit record</param>
        private int GetWeight(AuditRailDailySpiRule rule, AuditRailDaily record)
        {
            int weight = 0;
            if (rule.DestinationState == record.DestinationState) weight++;
            if (rule.SupplierCode == record.SupplierCode) weight++;
            if (rule.CustomsAttrib4 == record.CustomsAttrib4) weight++;
            return weight;
        }
    }
}

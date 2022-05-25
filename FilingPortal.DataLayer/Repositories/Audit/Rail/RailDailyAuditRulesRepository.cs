using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Repositories.Audit.Rail;
using Framework.DataLayer;
using Framework.Domain;

namespace FilingPortal.DataLayer.Repositories.Audit.Rail
{
    /// <summary>
    /// Repository of the <see cref="AuditRailDailyRule"/> entity
    /// </summary>
    class RailDailyAuditRulesRepository : SearchRepositoryWithTypedId<AuditRailDailyRule, int>, IRailDailyAuditRulesRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="RailDailyAuditRulesRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public RailDailyAuditRulesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The rule to be checked</param>
        public bool IsDuplicate(AuditRailDailyRule rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(AuditRailDailyRule rule)
        {
            return Set.Where(x => x.Id != rule.Id
                                           && x.ImporterCode == rule.ImporterCode
                                           && x.SupplierCode == rule.SupplierCode
                                           && x.ConsigneeCode == rule.ConsigneeCode
                                           && x.GoodsDescription == rule.GoodsDescription
                                           && x.Tariff == rule.Tariff
                                           && x.PortCode == rule.PortCode
                                           && x.DestinationState == rule.DestinationState
                                           && x.CustomsAttrib4 == rule.CustomsAttrib4)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
        
        /// <summary>
        /// Finds rule to validate against
        /// </summary>
        /// <param name="record">Rail daily audit record</param>
        public async Task<IList<AuditRailDailyRule>> FindCorrespondingRules(AuditRailDaily record)
        {
            var rules = await Set.Where(x =>
                x.ImporterCode == record.ImporterCode
                && record.GoodsDescription != null && record.GoodsDescription.Contains(x.GoodsDescription)
                && (string.IsNullOrEmpty(x.SupplierCode) || x.SupplierCode == record.SupplierCode)
                && (string.IsNullOrEmpty(x.ConsigneeCode) || x.ConsigneeCode == record.ConsigneeCode)
                && (string.IsNullOrEmpty(x.Tariff) || x.Tariff == record.Tariff)
                && (string.IsNullOrEmpty(x.PortCode) || x.PortCode == record.PortCode)
                && (string.IsNullOrEmpty(x.DestinationState) || x.DestinationState == record.DestinationState)
                && (string.IsNullOrEmpty(x.CustomsAttrib4) || x.CustomsAttrib4 == record.CustomsAttrib4)
                ).AsNoTracking().ToListAsync();

            List<AuditRailDailyRule> result = new List<AuditRailDailyRule>();
            var resultWeight = 0;

            foreach (var rule in rules)
            {
                var weight = GetWeight(rule, record);
                if (weight > resultWeight)
                {
                    result = new List<AuditRailDailyRule> {rule};
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
        private int GetWeight(AuditRailDailyRule rule, AuditRailDaily record)
        {
            var weight = 0;
            if (rule.CustomsAttrib4 == record.CustomsAttrib4) weight++;
            if (rule.DestinationState == record.DestinationState) weight++;
            if (rule.PortCode == record.PortCode) weight++;
            if (rule.SupplierCode == record.SupplierCode) weight++;
            if (rule.ConsigneeCode == record.ConsigneeCode) weight++;
            if (rule.Tariff == record.Tariff) weight++;
            return weight;
        }
    }
}

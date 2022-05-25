using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckExportRuleConsigneeRepository" />
    /// </summary>
    internal class TruckExportRuleConsigneeRepository : SearchRepositoryWithTypedId<TruckExportRuleConsignee, int>, IRuleRepository<TruckExportRuleConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleConsigneeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public TruckExportRuleConsigneeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(TruckExportRuleConsignee rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(TruckExportRuleConsignee rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.ConsigneeCode))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.ConsigneeCode.Trim() == rule.ConsigneeCode.Trim())
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id)
        {
            return Set.Any(x => x.Id == id);
        }
    }
}

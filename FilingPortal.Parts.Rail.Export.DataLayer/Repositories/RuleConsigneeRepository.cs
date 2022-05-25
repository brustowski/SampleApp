using System.Linq;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="RuleConsignee" />
    /// </summary>
    internal class RuleConsigneeRepository : SearchRepository<RuleConsignee>, IRuleRepository<RuleConsignee>
    {

        public int GetId(RuleConsignee rule) => default(int);
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleConsigneeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public RuleConsigneeRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RuleConsignee rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.ConsigneeCode)) return false;
            return Set.Any(x =>
                x.Id != rule.Id &&
                x.ConsigneeCode == rule.ConsigneeCode);
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

using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailRulePort"/> with <see cref="int"/> id
    /// </summary>
    public class RailRulePortRepository : SearchRepositoryWithTypedId<RailRulePort, int>, IRuleRepository<RailRulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRulePortRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailRulePortRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RailRulePort rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RailRulePort rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Port))
                return default(int);

            return Set.Where(x => x.Id != rule.Id && x.Port != null && x.Port.Trim() == rule.Port.Trim())
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        public bool IsExist(int id)
        {
            return Set.Any(x => x.Id == id);
        }
    }
}

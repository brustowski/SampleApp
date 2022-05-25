using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineRulePriceRepository"/>
    /// </summary>
    public class PipelineRulePriceRepository : SearchRepositoryWithTypedId<PipelineRulePrice, int>, IRuleRepository<PipelineRulePrice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulePriceRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineRulePriceRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(PipelineRulePrice rule)
        {

            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(PipelineRulePrice rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id &&
                                  x.ImporterId == rule.ImporterId &&
                                  x.CrudeTypeId == rule.CrudeTypeId &&
                                  x.FacilityId == rule.FacilityId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
    }
}

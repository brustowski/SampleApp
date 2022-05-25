using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineRuleConsigneeImporter"/>
    /// </summary>
    public class PipelineRuleConsigneeImporterRepository : SearchRepositoryWithTypedId<PipelineRuleConsigneeImporter, int>, IRuleRepository<PipelineRuleConsigneeImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleConsigneeImporterRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineRuleConsigneeImporterRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(PipelineRuleConsigneeImporter rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(PipelineRuleConsigneeImporter rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.ImporterFromTicket))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.ImporterFromTicket.Trim() == rule.ImporterFromTicket.Trim())
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

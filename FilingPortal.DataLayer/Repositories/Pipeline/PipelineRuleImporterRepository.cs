using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineRuleImporter"/>
    /// </summary>
    public class PipelineRuleImporterRepository : SearchRepositoryWithTypedId<PipelineRuleImporter, int>, IRuleRepository<PipelineRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleImporterRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineRuleImporterRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(PipelineRuleImporter rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(PipelineRuleImporter rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Importer))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.Importer != null && x.Importer.Trim() == rule.Importer.Trim())
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

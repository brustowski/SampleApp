using System.Linq;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="RuleImporter" />
    /// </summary>
    internal class RuleImporterRepository : SearchRepositoryWithTypedId<RuleImporter, int>, IRuleRepository<RuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleImporterRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public RuleImporterRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RuleImporter rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RuleImporter rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id
                                  && x.ImporterId == rule.ImporterId)
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

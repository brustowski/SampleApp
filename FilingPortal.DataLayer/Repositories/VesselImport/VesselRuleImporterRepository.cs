using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselRuleImporter"/> 
    /// </summary>
    internal class VesselRuleImporterRepository : SearchRepositoryWithTypedId<VesselRuleImporter, int>, IRuleRepository<VesselRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleImporterRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public VesselRuleImporterRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks if the specified rule duplicates any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(VesselRuleImporter rule) => GetId(rule) != default(int);

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(VesselRuleImporter rule) => (rule == null || string.IsNullOrWhiteSpace(rule.Importer))
            ? default(int)
            : Set.Where(x => x.Id != rule.Id && x.Importer.Trim() == rule.Importer.Trim())
            .Select(x => x.Id)
                .FirstOrDefault();

        /// <summary>
        /// Checks if the record with specified id exists
        /// </summary>
        /// <param name="id">Record id to check</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
    }
}

using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselRuleProduct" />
    /// </summary>
    internal class VesselRuleProductRepository : SearchRepositoryWithTypedId<VesselRuleProduct, int>, IRuleRepository<VesselRuleProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleProductRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public VesselRuleProductRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks if the specified rule duplicates any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(VesselRuleProduct rule) => GetId(rule) != default(int);


        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(VesselRuleProduct rule) => (rule == null || string.IsNullOrWhiteSpace(rule.Tariff))
            ? default(int)
            : Set.Where(x => x.Id != rule.Id && x.Tariff.Trim() == rule.Tariff.Trim())
                .Select(x => x.Id)
                .FirstOrDefault();

        /// <summary>
        /// Checks if the record with specified id exists
        /// </summary>
        /// <param name="id">Record id to check</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
    }
}

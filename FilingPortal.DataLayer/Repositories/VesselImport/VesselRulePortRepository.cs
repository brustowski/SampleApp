using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselRulePort"/> 
    /// </summary>
    internal class VesselRulePortRepository : SearchRepositoryWithTypedId<VesselRulePort, int>, IRuleRepository<VesselRulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public VesselRulePortRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks if the specified rule duplicates any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(VesselRulePort rule) => GetId(rule) != default(int);

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(VesselRulePort rule) => rule == null
            ? default(int)
            : Set.Where(x => x.Id != rule.Id && x.FirmsCodeId == rule.FirmsCodeId)
            .Select(x => x.Id)
            .FirstOrDefault();

        /// <summary>
        /// Checks if the record with specified id exists
        /// </summary>
        /// <param name="id">Record id to check</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);
    }
}

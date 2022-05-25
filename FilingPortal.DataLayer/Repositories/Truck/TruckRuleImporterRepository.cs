using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using FilingPortal.Domain.Repositories;
    using Framework.DataLayer;
    using System.Linq;

    /// <summary>
    /// Represents the repository of the <see cref="TruckRuleImporter" />
    /// </summary>
    internal class TruckRuleImporterRepository : SearchRepositoryWithTypedId<TruckRuleImporter, int>, IRuleRepository<TruckRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public TruckRuleImporterRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(TruckRuleImporter rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(TruckRuleImporter rule)
        {
            return rule != null
                ? default(int)
                : Set.Where(x => x.Id != rule.Id && x.CWIOR == rule.CWIOR && x.CWSupplier == rule.CWSupplier)
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

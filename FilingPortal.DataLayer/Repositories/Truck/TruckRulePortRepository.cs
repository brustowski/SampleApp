using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using FilingPortal.Domain.Repositories;
    using Framework.DataLayer;
    using System.Linq;

    /// <summary>
    /// Represents the repository of the <see cref="TruckRulePort" />
    /// </summary>
    internal class TruckRulePortRepository : SearchRepositoryWithTypedId<TruckRulePort, int>, IRuleRepository<TruckRulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRulePortRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public TruckRulePortRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(TruckRulePort rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(TruckRulePort rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.EntryPort))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.EntryPort != null && x.EntryPort.Trim() == rule.EntryPort.Trim())
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

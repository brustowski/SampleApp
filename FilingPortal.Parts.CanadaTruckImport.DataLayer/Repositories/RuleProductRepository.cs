using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="RuleProduct" />
    /// </summary>
    internal class RuleProductRepository : SearchRepositoryWithTypedId<RuleProduct, int>, IRuleRepository<RuleProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleProductRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public RuleProductRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RuleProduct rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RuleProduct rule)
        {
            if (rule == null)
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.ProductCodeId == rule.ProductCodeId)
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

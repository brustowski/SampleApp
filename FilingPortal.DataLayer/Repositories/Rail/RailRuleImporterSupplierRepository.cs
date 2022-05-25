using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailRuleImporterSupplier"/> with <see cref="id"/> id
    /// </summary>
    public class RailRuleImporterSupplierRepository : SearchRepositoryWithTypedId<RailRuleImporterSupplier, int>, IRailRuleImporterSupplierRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleImporterSupplierRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailRuleImporterSupplierRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RailRuleImporterSupplier rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RailRuleImporterSupplier rule)
        {
            if (string.IsNullOrEmpty(rule.ImporterName) || string.IsNullOrEmpty(rule.SupplierName))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id
                                  && x.ImporterName != null && x.SupplierName != null
                                  && x.ImporterName.Trim() == rule.ImporterName.Trim()
                                  && x.SupplierName.Trim() == rule.SupplierName.Trim() 
                                  && x.ProductDescription.Trim() == rule.ProductDescription.Trim() 
                                  && x.Port == rule.Port 
                                  && x.Destination == rule.Destination)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether rule with specified id exist
        /// </summary>
        /// <param name="id">The rule identifier</param>
        public bool IsExist(int id)
        {
            return Set.Any(x => x.Id == id);
        }

        /// <summary>
        /// Gets a list of importer names
        /// </summary>
        /// <param name="search">Search request</param>
        public IList<string> GetImporters(string search)
        {
            IQueryable<string> query = Set.AsQueryable().Select(x => x.ImporterName).Distinct(); ;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Contains(search));
            }

            return query.OrderBy(x => x.Trim()).ToList();
        }

        /// <summary>
        /// Gets a list of supplier names
        /// </summary>
        /// <param name="search">Search request</param>
        public IList<string> GetSuppliers(string search)
        {
            IQueryable<string> query = Set.AsQueryable().Select(x => x.SupplierName).Distinct();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Contains(search));
            }

            return query.OrderBy(x => x.Trim()).ToList();
        }
    }
}

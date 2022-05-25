using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailRuleDescription"/> with <see cref="id"/> id
    /// </summary>
    public class RailRuleDescriptionRepository : SearchRepositoryWithTypedId<RailRuleDescription, int>, IRailRuleProductDescriptionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleDescriptionRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailRuleDescriptionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(RailRuleDescription rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(RailRuleDescription rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Description1))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id
                                  && x.Description1 == rule.Description1
                                  && x.Importer == rule.Importer
                                  && x.Supplier == rule.Supplier
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
        /// Gets a list of descriptions
        /// </summary>
        /// <param name="search">Search request</param>
        public IList<string> GetDescriptions(string search)
        {
            IQueryable<string> query = Set.AsQueryable().Select(x => x.Description1).Distinct();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Contains(search));
            }

            return query.OrderBy(x => x.Trim()).ToList();
        }
    }
}

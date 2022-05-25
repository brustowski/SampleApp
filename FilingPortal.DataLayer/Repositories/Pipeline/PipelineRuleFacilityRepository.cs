using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{

    /// <summary>
    /// Represents the repository of the <see cref="PipelineRuleFacilityRepository"/>
    /// </summary>
    public class PipelineRuleFacilityRepository : SearchRepositoryWithTypedId<PipelineRuleFacility, int>,
        IRuleRepository<PipelineRuleFacility>,
        IDataProviderRepository<PipelineRuleFacility, int>,
        IPipelineRuleFacilityRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleFacilityRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineRuleFacilityRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(PipelineRuleFacility rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(PipelineRuleFacility rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.Facility))
            {
                return default(int);
            }

            return Set.Where(x =>
                x.Id != rule.Id &&
                x.Facility.Trim() == rule.Facility.Trim() &&
                (x.Port != null && rule.Port != null && x.Port.Trim() == rule.Port.Trim() ||
                 x.Port == null && rule.Port == null))
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Checks whether default value record with specified id exist
        /// </summary>
        /// <param name="id">The default value record identifier</param>
        public bool IsExist(int id) => Set.Any(x => x.Id == id);

        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<PipelineRuleFacility> Search(string searchInfo, int limit)
        {
            IQueryable<PipelineRuleFacility> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Facility.Contains(searchInfo));
            }

            return query.OrderBy(x => x.Facility).Take(limit).ToList();
        }

        /// <summary>
        /// Gets Facility rule by name
        /// </summary>
        /// <param name="name">Facility name</param>
        public PipelineRuleFacility GetByFacilityName(string name) => Set.FirstOrDefault(x => x.Facility.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}

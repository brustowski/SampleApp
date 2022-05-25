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
    /// Represents the repository of the <see cref="PipelineRuleBatchCode"/>
    /// </summary>
    public class PipelineRuleBatchCodeRepository :
        SearchRepositoryWithTypedId<PipelineRuleBatchCode, int>,
        IRuleRepository<PipelineRuleBatchCode>,
        IDataProviderRepository<PipelineRuleBatchCode, int>,
        IPipelineRuleBatchCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleBatchCodeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineRuleBatchCodeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks whether specified rule is not duplicating any other rule
        /// </summary>
        /// <param name="rule">The Rule to be checked</param>
        public bool IsDuplicate(PipelineRuleBatchCode rule)
        {
            return GetId(rule) != default(int);
        }

        /// <summary>
        /// Gets the duplicate rule
        /// </summary>
        /// <param name="rule">The rule to get</param>
        public int GetId(PipelineRuleBatchCode rule)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.BatchCode))
            {
                return default(int);
            }

            return Set.Where(x => x.Id != rule.Id && x.BatchCode != null && x.BatchCode.Trim() == rule.BatchCode.Trim())
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
        public IList<PipelineRuleBatchCode> Search(string searchInfo, int limit)
        {
            IQueryable<PipelineRuleBatchCode> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.BatchCode.Contains(searchInfo) || x.Product.Contains(searchInfo));
            }

            return query.OrderBy(x => x.BatchCode).Take(limit).ToList();
        }

        /// <summary>
        /// Gets Batch rule by code
        /// </summary>
        /// <param name="code">Batch code</param>
        public PipelineRuleBatchCode GetByBatchCode(string code) => Set.FirstOrDefault(x => x.BatchCode.Equals(code, StringComparison.InvariantCultureIgnoreCase));
    }
}

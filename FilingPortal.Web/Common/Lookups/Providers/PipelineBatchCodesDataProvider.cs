using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for Pipeline batch codes
    /// </summary>
    public class PipelineBatchCodesDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Pipeline batch codes repository
        /// </summary>
        private readonly IDataProviderRepository<PipelineRuleBatchCode, int> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineBatchCodesDataProvider"/> class
        /// </summary>
        /// <param name="repository">Pipeline batch codes repository</param>
        public PipelineBatchCodesDataProvider(IDataProviderRepository<PipelineRuleBatchCode, int> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.PipelineRuleBatchCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                PipelineRuleBatchCode record = _repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                    result.Add(new LookupItem { DisplayValue = $"{record.BatchCode} - {record.Product}", Value = record.Id });
                return result;
            }
            IEnumerable<PipelineRuleBatchCode> data = _repository.Search(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.BatchCode} - {x.Product}", Value = x.Id });
        }
    }
}
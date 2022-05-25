using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Inbond.Web.Common.Providers
{
    /// <summary>
    /// Provider for Marks and Remarks Templates names
    /// </summary>
    public class MarksRemarksDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Repository
        /// </summary>
        private readonly ISearchRepository<MarksRemarksTemplate> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksRemarksDataProvider"/> class
        /// </summary>
        /// <param name="repository">The repository</param>
        public MarksRemarksDataProvider(ISearchRepository<MarksRemarksTemplate> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.MarksRemarksTemplateTypes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return _repository.GetAll().Select(x => new LookupItem { DisplayValue = x.TemplateType, Value = x });
        }
    }
}
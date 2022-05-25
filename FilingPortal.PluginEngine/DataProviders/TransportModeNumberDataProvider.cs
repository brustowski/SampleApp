using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for importer lookup data provider
    /// </summary>
    public class TransportModeNumberDataProvider : ILookupDataProvider, IFilterDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly ITransportModeRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportModeNumberDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public TransportModeNumberDataProvider(ITransportModeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TransportModeNumber;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var results = new List<TransportMode>();

            if (searchInfo.SearchByKey)
            {
                TransportMode result = _repository.GetByNumber(searchInfo.Search);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            else
            {
                results.AddRange(_repository.SearchForUS(searchInfo.Search, searchInfo.Limit));
            }

            return results.Select(x => new LookupItem { DisplayValue = $"{x.CodeNumber} - {x.Description}", Value = x.CodeNumber });
        }
    }
}
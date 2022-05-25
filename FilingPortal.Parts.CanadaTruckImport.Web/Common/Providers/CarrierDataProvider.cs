using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Configs;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers
{
    /// <summary>
    /// Provider for Carriers
    /// </summary>
    public class CarrierDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Repository
        /// </summary>
        private readonly IDataProviderRepository<Carrier, string> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierDataProvider"/> class
        /// </summary>
        /// <param name="repository">The repository</param>
        public CarrierDataProvider(IDataProviderRepository<Carrier, string> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.Carriers;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var carriers = new List<Carrier>();

            if (searchInfo.SearchByKey)
            {
                Carrier carrier = _repository.Get(searchInfo.Search);
                if (carrier != null)
                {
                    carriers.Add(carrier);
                }
            }
            else
            {
                carriers.AddRange(_repository.Search(searchInfo.Search, searchInfo.Limit));
            }

            return carriers.Select(carrier => new LookupItem { DisplayValue = $"{carrier.Id} - {carrier.Name}", Value = carrier.Id });
        }
    }
}

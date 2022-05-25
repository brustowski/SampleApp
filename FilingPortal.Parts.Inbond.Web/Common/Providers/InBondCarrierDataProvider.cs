using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.Web.Common.Providers
{
    /// <summary>
    /// Provider for Marks and Remarks Templates names
    /// </summary>
    public class InBondCarrierDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Repository
        /// </summary>
        private readonly IDataProviderRepository<InBondCarrier, string> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InBondCarrierDataProvider"/> class
        /// </summary>
        /// <param name="repository">The repository</param>
        public InBondCarrierDataProvider(IDataProviderRepository<InBondCarrier, string> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.InBondCarriers;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var carriers = new List<InBondCarrier>();

            if (searchInfo.SearchByKey)
            {
                InBondCarrier carrier = _repository.Get(searchInfo.Search);
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

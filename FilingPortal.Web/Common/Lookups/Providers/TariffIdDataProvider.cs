using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Tariff data 
    /// </summary>
    public class TariffIdsDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Tariff tables repository
        /// </summary>
        private readonly ITariffRepository<HtsTariff> _repository;

        /// <summary>
        /// Initialize Tariff table repository
        /// </summary>
        /// <param name="repository">Tariff table Repository</param>
        public TariffIdsDataProvider(ITariffRepository<HtsTariff> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TariffIds;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrEmpty(searchInfo.Search))
            {
                HtsTariff record = _repository.Get(Convert.ToInt32(searchInfo.Search));
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = record.USC_Tariff, Value = record.Id });
                }

                return result;
            }

            IEnumerable<HtsTariff> data = _repository.GetTariffData(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.USC_Tariff, Value = x.Id });
        }
    }
}
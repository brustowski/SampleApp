using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Tariff data 
    /// </summary>
    public class TariffDataProvider : ILookupDataProvider, IUiAvailable, IFilterDataProvider
    {
        /// <summary>
        /// HTS Tariff repository
        /// </summary>
        private readonly ITariffRepository<HtsTariff> _htsRepository;

        /// <summary>
        /// SchB tariffs repository
        /// </summary>
        private readonly ITariffRepository<SchbTariff> _schbTariffRepository;

        /// <summary>
        /// Initialize Tariff table repository
        /// </summary>
        /// <param name="htsRepository">HTS Tariff table Repository</param>
        /// <param name="schbTariffRepository">SchB Tariff Repository</param>
        public TariffDataProvider(ITariffRepository<HtsTariff> htsRepository, ITariffRepository<SchbTariff> schbTariffRepository)
        {
            _htsRepository = htsRepository;
            _schbTariffRepository = schbTariffRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TariffCodes;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (string.IsNullOrWhiteSpace(searchInfo.DependValue) || searchInfo.DependValue == "HTS")
            {
                IEnumerable<HtsTariff> data = _htsRepository.GetTariffData(searchInfo.Search, searchInfo.Limit);
                return data.Select(x => new LookupItem { DisplayValue = x.USC_Tariff, Value = x.USC_Tariff });
            }

            if (searchInfo.DependValue == "SHB")
            {
                IEnumerable<SchbTariff> data = _schbTariffRepository.GetTariffData(searchInfo.Search, searchInfo.Limit);
                return data.Select(x => new LookupItem { DisplayValue = x.UB_Tariff, Value = x.UB_Tariff });
            }

            return new List<LookupItem>();
        }
    }
}
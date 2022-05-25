using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Country data 
    /// </summary>
    public class DischargePortCountryDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Country handbook repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// Foreign port repository
        /// </summary>
        private readonly IForeignPortsRepository _foreignPortRepository;

        /// <summary>
        /// Initialize Discharge Port Country data provider
        /// </summary>
        /// <param name="countryRepository">Country repository</param>
        /// <param name="foreignPortRepository">Foreign port repository</param>
        public DischargePortCountryDataProvider(ICountryRepository countryRepository
            , IForeignPortsRepository foreignPortRepository)
        {
            _countryRepository = countryRepository;
            _foreignPortRepository = foreignPortRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.DischargePortCountries;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                return GetExactRecord(searchInfo);
            }

            return string.IsNullOrWhiteSpace(searchInfo.DependOn)
                ? SearchRecords(searchInfo)
                : GetDependentRecord(searchInfo);
        }

        private IEnumerable<LookupItem> SearchRecords(SearchInfo searchInfo)
        {
            IEnumerable<Country> data = _countryRepository.Search(searchInfo.Search, searchInfo.Limit);
            return data.Map<Country, LookupItem>();
            //return data.Select(x => ConvertToLookupItem(x));
        }

        private IEnumerable<LookupItem> GetExactRecord(SearchInfo searchInfo)
        {
            Country record = _countryRepository.Get(Convert.ToInt32(searchInfo.Search));

            return record == null
                ? Enumerable.Empty<LookupItem>()
                : new[] { record.Map<Country, LookupItem>() };
        }

        private IEnumerable<LookupItem> GetDependentRecord(SearchInfo searchInfo)
        {
            if (string.IsNullOrWhiteSpace(searchInfo.DependValue))
            {
                return SearchRecords(searchInfo);
            }

            ForeignPort port = _foreignPortRepository.GetByCode(searchInfo.DependValue);
            if (port == null)
            {
                return SearchRecords(searchInfo);
            }

            Country country = _countryRepository.GetByCode(port.Country);
            if (country == null)
            {
                return SearchRecords(searchInfo);
            }
            LookupItem item = country.Map<Country, LookupItem>();
            item.IsDefault = true;
            return new[] { item };
        }
    }
}
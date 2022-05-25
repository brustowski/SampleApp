using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Tariff data 
    /// </summary>
    public class TariffTypeProvider : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TariffTypes;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new List<LookupItem>
            {
                new LookupItem { Value = "HTS", DisplayValue = "HTS" },
                new LookupItem { Value = "SHB", DisplayValue = "SHB" }
            };
        }
    }
}
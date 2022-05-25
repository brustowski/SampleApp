using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for Recon Values
    /// </summary>
    public class YesNoLookUpDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Get the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.YesNoLookup;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "Y - Yes", Value = "Y"},
                new LookupItem() {DisplayValue = "N - No", Value = "N"}
            };
        }
    }

}
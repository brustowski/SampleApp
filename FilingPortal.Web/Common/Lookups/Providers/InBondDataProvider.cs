using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents In-Bond data provider
    /// </summary>
    public class InBondDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Get the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.InBond;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "67 – Zone withdrawal for immediate export", Value = "67"},
                new LookupItem() {DisplayValue = "70 – Merchandise not shipped in-bond", Value = "70"}
            };
        }
    }

}
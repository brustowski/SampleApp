using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for Consignee Types
    /// </summary>
    public class ConsigneeTypeLookupDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Get the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ConsigneeTypeLookup;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "D - Direct Consumer", Value = "D"},
                new LookupItem() {DisplayValue = "G - Government Entity", Value = "G"},
                new LookupItem() {DisplayValue = "O - Other/Unknown", Value = "O"},
                new LookupItem() {DisplayValue = "R - Reseller", Value = "R"}
            };
        }
    }

}
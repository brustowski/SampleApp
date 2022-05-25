using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{

    /// <summary>
    /// Represents provider for Recon Values
    /// </summary>
    public class ValueReconeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ValueRecons;

        /// <summary>
        /// Gets the collection of value recons
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "NA  - Not Applicable", Value = "NA"},
                new LookupItem() {DisplayValue = "98  - 9802 recon", Value = "98"},
                new LookupItem() {DisplayValue = "C9  - Class/9802 recon", Value = "C9"},
                new LookupItem() {DisplayValue = "CL  - Class Recon", Value = "CL"},
                new LookupItem() {DisplayValue = "V9  - Value/9802 recon", Value = "V9"},
                new LookupItem() {DisplayValue = "AL  - Value/Class/9802 recon", Value = "AL"},
                new LookupItem() {DisplayValue = "VC  - Value/Class Recon", Value = "VC"},
                new LookupItem() {DisplayValue = "VL  - Value Recon", Value = "VL"}

            };
        }
    }
}

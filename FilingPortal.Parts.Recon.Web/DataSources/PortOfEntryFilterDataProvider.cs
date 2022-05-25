using System.Collections.Generic;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Represents the Port of Entry Filter Data provider
    /// </summary>
    public class PortOfEntryFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of filter items
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "All", Value = null },
                new LookupItem() {DisplayValue = "Puerto Rico", Value = PortOfEntryFilterValues.PuertoRico },
                new LookupItem() {DisplayValue = "Mainland US", Value = PortOfEntryFilterValues.MainlandUS },
            };
        }
    }
}
using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for Validation passed Filter data
    /// </summary>
    public class ValidationPassedFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of filter items
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "All", Value = null},
                new LookupItem() {DisplayValue = "True", Value = true},
                new LookupItem() {DisplayValue = "False", Value = false}
            };
        }
    }
}
using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for FilingStatus data
    /// </summary>
    public class ClientStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of Client Status items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "All", Value = null},
                new LookupItem() {DisplayValue = "Active", Value = true},
                new LookupItem() {DisplayValue = "Deactivated", Value = false}
            };
        }
    }
}
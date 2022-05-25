using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for boolean data
    /// </summary>
    public class FileUploadMethodFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "All", Value = null},
                new LookupItem() {DisplayValue = "Automated", Value = true},
                new LookupItem() {DisplayValue = "Manual", Value = false}
            };
        }
    }
}
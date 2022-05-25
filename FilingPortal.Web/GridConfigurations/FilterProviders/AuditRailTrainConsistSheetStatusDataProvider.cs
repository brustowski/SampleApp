using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for rail audit statuses
    /// </summary>
    public class AuditRailTrainConsistSheetStatusDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem {DisplayValue = "All", Value = null},
                new LookupItem {DisplayValue = "Open", Value = "Open"},
                new LookupItem {DisplayValue = "Not found", Value = "Not found"},
                new LookupItem {DisplayValue = "Not matched", Value = "Not matched"},
                new LookupItem {DisplayValue = "Matched", Value = "Matched"},
            };
        }
    }
}
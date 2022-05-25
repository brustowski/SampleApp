using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents provider for Inbound Records error in rule
    /// </summary>
    public class ErrorStatusDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem {DisplayValue = "All"},
                new LookupItem {DisplayValue = "Rules found", Value = "true"},
                new LookupItem {DisplayValue = "No rules found", Value = "false"}
            };
        }
    }

}
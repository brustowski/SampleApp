using System.Collections.Generic;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// Abstract class for filter DataProvider
    /// </summary>
    public abstract class FilterDataProviderBase : IFilterDataProvider
    {
        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public abstract IEnumerable<LookupItem> GetData(SearchInfo searchInfo);
    }
}
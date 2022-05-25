using System.Collections.Generic;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// Interface for filter DataProvider
    /// </summary>
    public interface IFilterDataProvider: IDataProvider
    {
        
    }

    public interface IDataProvider
    {
        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        IEnumerable<LookupItem> GetData(SearchInfo searchInfo);
    }
}
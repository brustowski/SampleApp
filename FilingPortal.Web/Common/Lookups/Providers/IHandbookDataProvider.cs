using System.Collections.Generic;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Describes handbook lookup data provider
    /// </summary>
    public interface IHandbookDataProvider
    {
        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="handbookName">Handbook name</param>
        /// <param name="searchInfo">The search information</param>
        IEnumerable<LookupItem<string>> GetData(string handbookName, SearchInfo searchInfo);
    }
}
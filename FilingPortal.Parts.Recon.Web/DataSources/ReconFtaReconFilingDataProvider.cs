using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Provider for Recon FTA Recon Filing
    /// </summary>
    public class ReconFtaReconFilingDataProvider : IFilterDataProvider
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
                new LookupItem() {DisplayValue = "Empty", Value = ""},
                new LookupItem() {DisplayValue = "FILE", Value = "FILE"},
                new LookupItem() {DisplayValue = "EXPIRE", Value = "EXPIRE"},
            };
        }
    }
}
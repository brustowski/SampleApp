using System.Collections.Generic;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Provider for Recon Status
    /// </summary>
    public class ReconFtaDataProvider : IFilterDataProvider
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
                new LookupItem() {DisplayValue = "Blank", Value = ""},
                new LookupItem() {DisplayValue = "Yes", Value = "Y"},
            };
        }
    }
}
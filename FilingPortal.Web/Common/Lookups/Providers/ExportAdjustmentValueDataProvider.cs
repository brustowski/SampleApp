using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents Export Adjustment Value data provider
    /// </summary>
    public class ExportAdjustmentValueDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Get the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ExportAdjustmentValue;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "P - Prelim", Value = "P"},
                new LookupItem() {DisplayValue = "F - Final", Value = "F"}
            };
        }
    }

}
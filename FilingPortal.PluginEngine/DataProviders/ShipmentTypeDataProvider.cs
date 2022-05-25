using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents the data provider for the shipment type
    /// </summary>
    public class ShipmentTypeDataProvider : ILookupDataProvider, IFilterDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ShipmentTypes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var results = new List<LookupItem>()
            {
                new LookupItem {DisplayValue = "All", Value = null},
                new LookupItem {DisplayValue = "DRW - Drawback", Value = "DRW"},
                new LookupItem {DisplayValue = "EXP - Export", Value = "EXP"},
                new LookupItem {DisplayValue = "FTZ - Foreign Trade Zone", Value = "FTZ"},
                new LookupItem {DisplayValue = "IMP - Import", Value = "IMP"},
                new LookupItem {DisplayValue = "IMX - Import by external broker", Value = "IMX"},
                new LookupItem {DisplayValue = "MSC - Miscellaneous Customs", Value = "MSC"},
                new LookupItem {DisplayValue = "REC - Reconciliation", Value = "REC"},
            };

            if (searchInfo.SearchByKey)
            {
                var result = results.FirstOrDefault(x => (string) x.Value == searchInfo.Search);
                return new[] {result};
            }

            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
                results = results.Where(x => x.DisplayValue.ToLower().Contains(searchInfo.Search.ToLower())).ToList();

            return results;
        }
    }
}
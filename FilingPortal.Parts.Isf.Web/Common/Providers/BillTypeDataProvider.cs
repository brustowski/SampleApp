using System.Collections.Generic;
using FilingPortal.Parts.Isf.Web.Configs;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Isf.Web.Common.Providers
{
    internal class BillTypeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.BillTypes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new List<LookupItem>
            {
                new LookupItem {DisplayValue = "MB - Master Bill", Value = "MB"},
                new LookupItem {DisplayValue = "BM - House Bill", Value = "BM"},
                new LookupItem {DisplayValue = "OB - Ocean Bill", Value = "OB"},
            };
        }
    }
}

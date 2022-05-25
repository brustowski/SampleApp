using System.Collections.Generic;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for FilingStatus data
    /// </summary>
    public class ClientTypeFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of Client type items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "All", Value = ClientType.None},
                new LookupItem() {DisplayValue = ClientType.Importer.GetDescription(), Value = ClientType.Importer},
                new LookupItem() {DisplayValue = ClientType.Supplier.GetDescription(), Value = ClientType.Supplier}
            };
        }
    }
}
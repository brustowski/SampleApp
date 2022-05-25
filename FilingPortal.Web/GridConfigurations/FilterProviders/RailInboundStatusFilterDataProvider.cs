using System.Collections.Generic;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Web.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for Status data
    /// </summary>
    public class RailInboundStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Gets the collection of Client type items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = RailInboundRecordStatus.Open.GetDescription(), Value = RailInboundRecordStatus.Open},
                new LookupItem() {DisplayValue = RailInboundRecordStatus.Duplicates.GetDescription(), Value = RailInboundRecordStatus.Duplicates},
                new LookupItem() {DisplayValue = RailInboundRecordStatus.Archived.GetDescription(), Value = RailInboundRecordStatus.Archived},
                new LookupItem() {DisplayValue = RailInboundRecordStatus.Deleted.GetDescription(), Value = RailInboundRecordStatus.Deleted}
            };
        }
    }
}
using System.Collections.Generic;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for Freight Type
    /// </summary>
    public class FreightTypeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Get the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.FreightTypes;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem {DisplayValue = FreightType.PerContainer.GetDescription(), Value = (int)FreightType.PerContainer},
                new LookupItem {DisplayValue = FreightType.PerUom.GetDescription(), Value = (int)FreightType.PerUom}
            };
        }
    }

}
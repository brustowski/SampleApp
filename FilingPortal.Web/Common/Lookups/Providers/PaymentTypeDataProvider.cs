using Framework.Domain.Paging;
using System.Collections.Generic;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for Payment Types
    /// </summary>
    public class PaymentTypeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.PaymentTypes;

        /// <summary>
        /// Gets the collection of payment types 
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return new[]
            {
                new LookupItem() {DisplayValue = "1  - Payments to be made on Individual basis", Value = "1"},
                new LookupItem() {DisplayValue = "2  - Payments to be batched by preliminary statement print date and entry filer code", Value = "2"},
                new LookupItem() {DisplayValue = "3  - Payments to be batched by preliminary statement print date and importer of record number", Value = "3"},
                new LookupItem() {DisplayValue = "5  - Payments to be batched by preliminary statement print date and importer of record number for an importer with several subdivisions", Value = "5"},
                new LookupItem() {DisplayValue = "6  - Payments to be batched by preliminary periodic daily statement print date and entry filer code", Value = "6"},
                new LookupItem() {DisplayValue = "7  - Payments to be batched by preliminary periodic daily statement print date and importer of record number", Value = "7"},
                new LookupItem() {DisplayValue = "8  - Payments to be batched by preliminary periodic daily statement print date and importer of record number for an importer with several subdivisions", Value = "8"}

            };
        }
    }
}

using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Provider for Recon to ACE comparison errors
    /// </summary>
    public class ReconAceComparisonErrorsDataProvider : IFilterDataProvider
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
                new LookupItem() {DisplayValue = ErrorType.All.GetDescription(), Value = ErrorType.All},
                new LookupItem() {DisplayValue = ErrorType.ReconValueFlag.GetDescription(), Value = ErrorType.ReconValueFlag},
                new LookupItem() {DisplayValue = ErrorType.ReconFtaFlag.GetDescription(), Value = ErrorType.ReconFtaFlag},
                new LookupItem() {DisplayValue = ErrorType.EntryValue.GetDescription(), Value = ErrorType.EntryValue},
                new LookupItem() {DisplayValue = ErrorType.Duty.GetDescription(), Value = ErrorType.Duty},
                new LookupItem() {DisplayValue = ErrorType.Mpf.GetDescription(), Value = ErrorType.Mpf},
                new LookupItem() {DisplayValue = ErrorType.PayableMpf.GetDescription(), Value = ErrorType.PayableMpf},
                new LookupItem() {DisplayValue = ErrorType.Quantity.GetDescription(), Value = ErrorType.Quantity},
                new LookupItem() {DisplayValue = ErrorType.Hts.GetDescription(), Value = ErrorType.Hts},
            };
        }
    }
}